using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[System.Serializable]
public class TetrominoMaterialEntry
{
    public TetrominoType blockType;
    public Material[] materials;
}

public class BlockController : MonoBehaviour
{
    private static BlockController instance;
    public static BlockController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<BlockController>();
                if (instance == null)
                {
                    GameObject go = new GameObject("BlockController");
                    instance = go.AddComponent<BlockController>();
                }
            }
            return instance;
        }
    }
    
    public CylindricalGrid grid;
    public CylinderRotator rotator;
    public float fallInterval = 1f;
    public float fastFallInterval = 0.1f;
    public bool isFastFalling = false;
    public int score = 0;
    public GameObject blockPrefab; // 俄罗斯方块单元格预制体
    public int ring;
    [Header("方块生成设置")]
    public int spawnHeightOffset = 3; // 方块生成高度偏移（游戏区域顶部往上几层）
    private Tetromino currentTetromino;
    private float fallTimer = 0f;
    private bool _isActive = false;
    private List<BlockInfo> activeCells = new List<BlockInfo>(); // 当前下落方块的格子实例
    public static Action<int> updateScore;
    public static Action<int> onLineClear; // 消除行时的事件
    private Queue<TetrominoType> nextQueue = new Queue<TetrominoType>();
    public static System.Action<List<TetrominoType>> updateNextQueue;
    
    // 公开isActive属性，供CylinderRotator访问
    public bool isActive 
    { 
        get { return _isActive; } 
        private set { _isActive = value; } 
    }
    
    [Header("Material Configuration")]
    [SerializeField] private TetrominoMaterialEntry[] materialEntries;
    public Dictionary<TetrominoType, Material[]> materials = new Dictionary<TetrominoType, Material[]>();
    
    private bool positionUpdateNeeded = false;
    
    // 缓存队列List，避免频繁创建
    private List<TetrominoType> cachedQueueList = new List<TetrominoType>();
    
    // 方块对象池
    private Queue<BlockInfo> blockPool = new Queue<BlockInfo>();
    private const int POOL_SIZE = 20; // 对象池大小
    
    void Awake()
    {
        // 单例逻辑
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        // 初始化对象池
        InitializeBlockPool();
        
        // 初始化队列
        nextQueue.Clear();
        int typeCount = System.Enum.GetValues(typeof(TetrominoType)).Length;
        for (int i = 0; i < 2; i++)
            nextQueue.Enqueue((TetrominoType)Random.Range(0, typeCount));
        foreach (var VARIABLE in materialEntries)
        {
            materials.Add(VARIABLE.blockType,VARIABLE.materials);
        }
    }

    void InitializeBlockPool()
    {
        for (int i = 0; i < POOL_SIZE; i++)
        {
            var go = Instantiate(blockPrefab, transform);
            go.SetActive(false);
            var blockInfo = go.GetComponent<BlockInfo>();
            if (blockInfo == null)
            {
                blockInfo = go.AddComponent<BlockInfo>();
            }
            blockPool.Enqueue(blockInfo);
        }
    }
    
    BlockInfo GetBlockFromPool()
    {
        if (blockPool.Count > 0)
        {
            var block = blockPool.Dequeue();
            block.gameObject.SetActive(true);
            return block;
        }
        else
        {
            // 池子空了，创建新的
            var go = Instantiate(blockPrefab, transform);
            var blockInfo = go.GetComponent<BlockInfo>();
            if (blockInfo == null)
            {
                blockInfo = go.AddComponent<BlockInfo>();
            }
            return blockInfo;
        }
    }
    
    void ReturnBlockToPool(BlockInfo block)
    {
        if (block != null)
        {
            block.gameObject.SetActive(false);
            block.transform.SetParent(transform);
            blockPool.Enqueue(block);
        }
    }

    void OnDestroy()
    {
        // 清理单例引用
        if (instance == this)
        {
            instance = null;
        }
    }

    void Update()
    {
        if (!isActive) return;
        fallTimer += Time.deltaTime;
        float interval = isFastFalling ? fastFallInterval : fallInterval;
        if (fallTimer >= interval)
        {
            fallTimer = 0f;
            if (CanPlaceWithRotation(currentTetromino.layer - 1, currentTetromino.rotation))
            {
                currentTetromino.layer -= 1;
                positionUpdateNeeded = true; // 标记需要更新位置
            }
            else
            {
                PlaceToGrid();
                int cleared = grid.CheckAndClearRings(out int chain);
                if (cleared > 0)
                {
                    // 新的消除行分数计算
                    int clearScore = 0;
                    if (cleared == 1)
                        clearScore = 100;
                    else if (cleared == 2)
                        clearScore = 300;
                    else if (cleared >= 3)
                        clearScore = 600;
                    
                    score += clearScore;
                    updateScore?.Invoke(score);
                    
                    // 触发消除行UI提示
                    onLineClear?.Invoke(cleared);
                    
                    // 播放消除行音效
                    if (AudioManager.Instance != null)
                    {
                        AudioManager.Instance.PlayLineClearSound();
                    }
                }
                if (IsGameOver())
                {
                    Debug.Log("Game Over! Final Score: " + score);
                    isActive = false;
                    
                    // 清理当前下落的方块
                    ClearCurrentTetromino();
                    
                    // 播放游戏结束音效
                    if (AudioManager.Instance != null)
                    {
                        AudioManager.Instance.PlayGameOverSound();
                    }
                    
                    if (UIManager.Instance != null)
                    {
                        UIManager.Instance.ShowGameOver(score);
                    }
                    return;
                }
                SpawnNewTetromino();
            }
        }
        
        // 只在需要时更新位置
        if (positionUpdateNeeded && isActive && activeCells.Count > 0)
        {
            UpdateActiveCellsPosition();
            positionUpdateNeeded = false;
        }
    }

    void SpawnNewTetromino()
    {
        // 取出队首
        TetrominoType type = nextQueue.Dequeue();
        // 补充新随机
        int typeCount = System.Enum.GetValues(typeof(TetrominoType)).Length;
        nextQueue.Enqueue((TetrominoType)Random.Range(0, typeCount));
        int rotation = 0;
        int layer = grid.layerCount + spawnHeightOffset; // 从游戏区域顶部再往上指定层数开始生成 
        int ring = this.ring;
        currentTetromino = new Tetromino(type, rotation, layer, ring);
        isActive = true;
        fallTimer = 0f;
        isFastFalling = false;
        CreateActiveCells();

        // 通知UI刷新 - 使用缓存的List
        cachedQueueList.Clear();
        cachedQueueList.AddRange(nextQueue);
        updateNextQueue?.Invoke(cachedQueueList);
    }

    void CreateActiveCells()
    {
        // 先回收旧的到对象池
        foreach (var blockInfo in activeCells)
        {
            ReturnBlockToPool(blockInfo);
        }
        activeCells.Clear();
        
        int[,] shape = currentTetromino.GetShape();
        for (int y = 0; y < 3; y++)
        for (int x = 0; x < 3; x++)
        {
            if (shape[y, x] == 0) continue;
            
            // 从对象池获取方块
            var blockInfo = GetBlockFromPool();
            blockInfo.SetInfo(currentTetromino.type, currentTetromino.rotation, 
                            currentTetromino.layer, currentTetromino.ring + x, true);
            
            activeCells.Add(blockInfo);
        }
        
        // 标记需要更新位置
        positionUpdateNeeded = true;
    }

    void UpdateActiveCellsPosition()
    {
        if (currentTetromino == null) return;
        
        int[,] shape = currentTetromino.GetShape();
        int idx = 0;
        
        // 预计算一些常用值
        float angleStep = 360f / grid.ringCount;
        float heightOffset = grid.cellHeight;
        
        for (int y = 0; y < 3; y++)
        for (int x = 0; x < 3; x++)
        {
            if (shape[y, x] == 0) continue;
            
            int gridLayer = currentTetromino.layer + y;
            int gridRing = (currentTetromino.ring + x) % grid.ringCount;
            
            // 优化角度计算
            float angle = gridRing * angleStep;
            float rad = angle * Mathf.Deg2Rad;
            float px = Mathf.Cos(rad) * grid.radius;
            float pz = Mathf.Sin(rad) * grid.radius;
            float py = gridLayer * heightOffset - 0.5f;
            
            var blockInfo = activeCells[idx++];
            var transform = blockInfo.transform;
            
            // 直接设置位置，避免创建新的Vector3
            transform.position = new Vector3(px, py, pz);
            transform.LookAt(new Vector3(0, py, 0), Vector3.up);
            
            // 直接调用BlockInfo的SetPosition，避免重复的GetComponent
            blockInfo.SetPosition(gridLayer, gridRing);
        }
    }
    
    // 获取考虑圆柱旋转后的实际ring值
    int GetActualRingForPlacement()
    {
        if (rotator == null) return currentTetromino.ring;
        
        // 计算圆柱旋转对应的ring偏移
        int ringOffset = rotator.GetRingOffset();
        
        // 返回考虑旋转后的实际ring值（减去偏移，因为圆柱旋转了，但方块位置没变）
        int actualRing = (currentTetromino.ring - ringOffset) % grid.ringCount;
        if (actualRing < 0) actualRing += grid.ringCount;
        return actualRing;
    }

    public bool TryRotate()
    {
        int newRot = (currentTetromino.rotation + 1) % 4;
        if (CanPlaceWithRotation(currentTetromino.layer, newRot))
        {
            currentTetromino.rotation = newRot;
            positionUpdateNeeded = true; // 标记需要更新位置
            return true;
        }
        return false;
    }

    public bool CanPlace(int layer, int ring, int rotation)
    {
        int[,] shape = TetrominoData.Shapes[currentTetromino.type][rotation];
        for (int y = 0; y < 3; y++)
        for (int x = 0; x < 3; x++)
        {
            if (shape[y, x] == 0) continue;
            int gridLayer = layer + y - 1;
            int gridRing = (ring + x) % grid.ringCount;
            
            // 只检查在游戏区域内的部分
            if (gridLayer < 0) return false; // 低于游戏区域底部不允许
            if (gridLayer >= grid.layerCount) continue; // 高于游戏区域顶部时跳过检查
            if (grid.grid[gridLayer, gridRing] != 0) return false;
        }
        return true;
    }
    
    // 检查是否可以放置（考虑圆柱旋转）
    bool CanPlaceWithRotation(int layer, int rotation)
    {
        // 获取考虑旋转后的实际ring值
        int actualRing = GetActualRingForPlacement();
        return CanPlace(layer, actualRing, rotation);
    }

    void PlaceToGrid()
    {
        int[,] shape = currentTetromino.GetShape();
        int idx = 0;
        int actualRing = GetActualRingForPlacement();
        
        for (int y = 0; y < 3; y++)
        for (int x = 0; x < 3; x++)
        {
            if (shape[y, x] == 0) continue;
            int gridLayer = currentTetromino.layer + y -1;
            int gridRing = (actualRing + x) % grid.ringCount;
            if (gridLayer >= 0 && gridLayer < grid.layerCount)
            {
                grid.SetCell(gridLayer, gridRing, true);
            }
            if (gridLayer >= 0 && gridLayer < grid.layerCount)
            {
                var cellObj = grid.cellObjects[gridLayer, gridRing];
                if (idx < activeCells.Count)
                {
                    activeCells[idx].transform.SetParent(cellObj.transform);
                    activeCells[idx].transform.localPosition = Vector3.zero;
                    activeCells[idx].transform.localRotation = Quaternion.identity;
                    activeCells[idx].SetActive(false);
                }
                idx++;
            }
        }
        activeCells.Clear();
        
        // 方块放置得分：每放置一次得2分
        score += 2;
        updateScore?.Invoke(score);
        
        // 播放方块落地音效
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayBlockLandSound();
        }
    }
    
    public bool IsGameOver()
    {
        // 顶层有方块则GameOver
        for (int i = 0; i < grid.ringCount; i++)
            if (grid.grid[grid.layerCount - 1, i] != 0)
                return true;
        return false;
    }

    public void StartGame()
    {
        if (isActive) return; // 已经在游戏中则不重复开始
        ClearAllBlocks(); // 清理所有方块和子对象
        score = 0;
        isActive = true;
        fallTimer = 0f;
        if (grid != null)
        {
            grid.ClearGrid();
        }
        SpawnNewTetromino();
        updateScore?.Invoke(score);
    }

    public void RestartGame()
    {
        ClearAllBlocks(); // 清理所有方块和子对象
        score = 0;
        isActive = true;
        fallTimer = 0f;
        isFastFalling = false;
        if (grid != null)
        {
            grid.ClearGrid();
        }
        SpawnNewTetromino();
        updateScore?.Invoke(score);
    }
    
    // 清理BlockController中的所有方块和子对象
    void ClearAllBlocks()
    {
        // 停止游戏逻辑
        isActive = false;
        
        // 清理当前下落的方块
        ClearCurrentTetromino();
        
        // 清理BlockController Transform下的所有子对象 - 避免创建临时List
        var transforms = transform.GetComponentsInChildren<Transform>();
        for (int i = 1; i < transforms.Length; i++) // 跳过自身(index 0)
        {
            if (transforms[i] != transform)
            {
                Destroy(transforms[i].gameObject);
            }
        }
    }
    
    // 仅清理当前下落的方块
    void ClearCurrentTetromino()
    {
        foreach (var blockInfo in activeCells)
        {
            ReturnBlockToPool(blockInfo);
        }
        activeCells.Clear();
        currentTetromino = null;
    }

    public void SetFastFalling(bool fast)
    {
        isFastFalling = fast;
    }

    // 响应圆柱旋转
    public void OnCylinderRotated()
    {
        if (isActive && activeCells.Count > 0)
        {
            positionUpdateNeeded = true;
        }
    }

    // 提供给UI获取队列 - 使用缓存避免创建新List
    public List<TetrominoType> GetNextQueue()
    {
        cachedQueueList.Clear();
        cachedQueueList.AddRange(nextQueue);
        return cachedQueueList;
    }

    // 公开获取当前方块的方法
    public Tetromino GetCurrentTetromino()
    {
        return currentTetromino;
    }
}

public static class CylindricalGridExtensions
{
    // 检查并清除满环，返回消除层数和连锁数
    public static int CheckAndClearRings(this CylindricalGrid grid, out int chain)
    {
        int cleared = 0;
        chain = 0;
        
        // 先找出所有需要清除的行，记录原始位置
        List<int> layersToRemove = new List<int>();
        
        for (int y = 0; y < grid.layerCount; y++)
        {
            bool full = true;
            for (int i = 0; i < grid.ringCount; i++)
            {
                if (grid.grid[y, i] == 0)
                {
                    full = false;
                    break;
                }
            }
            
            if (full)
            {
                layersToRemove.Add(y);
            }
        }
        
        // 如果没有需要清除的行，直接返回
        if (layersToRemove.Count == 0)
        {
            return 0;
        }
        
        // 先为每个原始位置生成特效
        foreach (int layer in layersToRemove)
        {
            if (grid.ringClearEffectPrefab != null)
            {
                float yPos = layer * grid.cellHeight;
                GameObject fx = GameObject.Instantiate(grid.ringClearEffectPrefab, grid.transform);
                fx.transform.localEulerAngles = new Vector3(-90, 0, 0);
                fx.transform.localPosition = new Vector3(0, yPos, 0);
            }
        }
        
        // 从底部开始清除行（不包括特效生成）
        for (int i = 0; i < layersToRemove.Count; i++)
        {
            int layerToRemove = layersToRemove[i] - i; // 考虑之前已经清除的行数
            grid.ClearRingWithoutEffect(layerToRemove);
            cleared++;
            chain++;
        }
        
        return cleared;
    }

    // 清除指定层并整体下落（不生成特效）
    public static void ClearRingWithoutEffect(this CylindricalGrid grid, int layer)
    {
        // 销毁被清除层的所有方块GameObject
        for (int i = 0; i < grid.ringCount; i++)
        {
            var cellObj = grid.cellObjects[layer, i];
            if (cellObj != null)
            {
                // 销毁所有子对象（方块）
                int childCount = cellObj.transform.childCount;
                if (childCount > 0)
                {
                    // 从后往前删除，避免索引变化问题
                    for (int j = childCount - 1; j >= 0; j--)
                    {
                        MonoBehaviour.Destroy(cellObj.transform.GetChild(j).gameObject);
                    }
                }
            }
            grid.grid[layer, i] = 0;
        }
        
        // 上方整体下落
        for (int y = layer; y < grid.layerCount - 1; y++)
        for (int i = 0; i < grid.ringCount; i++)
        {
            grid.grid[y, i] = grid.grid[y + 1, i];
            // 移动方块GameObject
            var currentCell = grid.cellObjects[y, i];
            var upperCell = grid.cellObjects[y + 1, i];
            if (currentCell != null && upperCell != null)
            {
                // 直接移动子对象，避免创建临时List
                int childCount = upperCell.transform.childCount;
                for (int j = childCount - 1; j >= 0; j--)
                {
                    var child = upperCell.transform.GetChild(j);
                    child.SetParent(currentCell.transform);
                    child.localPosition = Vector3.zero;
                    child.localRotation = Quaternion.identity;
                }
            }
        }
        
        // 顶层清空
        for (int i = 0; i < grid.ringCount; i++)
        {
            grid.grid[grid.layerCount - 1, i] = 0;
            var topCell = grid.cellObjects[grid.layerCount - 1, i];
            if (topCell != null)
            {
                // 销毁顶层所有方块
                int childCount = topCell.transform.childCount;
                for (int j = childCount - 1; j >= 0; j--)
                {
                    MonoBehaviour.Destroy(topCell.transform.GetChild(j).gameObject);
                }
            }
        }
    }

    // 清除指定层并整体下落
    public static void ClearRing(this CylindricalGrid grid, int layer)
    {
        // 销毁被清除层的所有方块GameObject
        for (int i = 0; i < grid.ringCount; i++)
        {
            var cellObj = grid.cellObjects[layer, i];
            if (cellObj != null)
            {
                // 销毁所有子对象（方块）
                int childCount = cellObj.transform.childCount;
                if (childCount > 0)
                {
                    // 从后往前删除，避免索引变化问题
                    for (int j = childCount - 1; j >= 0; j--)
                    {
                        MonoBehaviour.Destroy(cellObj.transform.GetChild(j).gameObject);
                    }
                }
            }
            grid.grid[layer, i] = 0;
        }
        
        // === 光圈特效 ===
        if (grid.ringClearEffectPrefab != null)
        {
            float yPos = layer * grid.cellHeight;
            GameObject fx = GameObject.Instantiate(grid.ringClearEffectPrefab, grid.transform);
            fx.transform.localEulerAngles = new Vector3(-90, 0, 0);
            fx.transform.localPosition = new Vector3(0, yPos, 0);
        }
        
        // 上方整体下落
        for (int y = layer; y < grid.layerCount - 1; y++)
        for (int i = 0; i < grid.ringCount; i++)
        {
            grid.grid[y, i] = grid.grid[y + 1, i];
            // 移动方块GameObject
            var currentCell = grid.cellObjects[y, i];
            var upperCell = grid.cellObjects[y + 1, i];
            if (currentCell != null && upperCell != null)
            {
                // 直接移动子对象，避免创建临时List
                int childCount = upperCell.transform.childCount;
                for (int j = childCount - 1; j >= 0; j--)
                {
                    var child = upperCell.transform.GetChild(j);
                    child.SetParent(currentCell.transform);
                    child.localPosition = Vector3.zero;
                    child.localRotation = Quaternion.identity;
                }
            }
        }
        
        // 顶层清空
        for (int i = 0; i < grid.ringCount; i++)
        {
            grid.grid[grid.layerCount - 1, i] = 0;
            var topCell = grid.cellObjects[grid.layerCount - 1, i];
            if (topCell != null)
            {
                // 销毁顶层所有方块
                int childCount = topCell.transform.childCount;
                for (int j = childCount - 1; j >= 0; j--)
                {
                    MonoBehaviour.Destroy(topCell.transform.GetChild(j).gameObject);
                }
            }
        }
    }
} 
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BlockController : MonoBehaviour
{
    public CylindricalGrid grid;
    public CylinderRotator rotator;
    public float fallInterval = 1f;
    public float fastFallInterval = 0.1f;
    public bool isFastFalling = false;
    public int score = 0;
    public GameObject blockPrefab; // 俄罗斯方块单元格预制体
    public int ring;
    private Tetromino currentTetromino;
    private float fallTimer = 0f;
    private bool isActive = false;
    private List<BlockInfo> activeCells = new List<BlockInfo>(); // 当前下落方块的格子实例
    public static Action<int> updateScore;
    public static Action<int> onLineClear; // 消除行时的事件
    private Queue<TetrominoType> nextQueue = new Queue<TetrominoType>();
    public static System.Action<List<TetrominoType>> updateNextQueue;

    void Awake()
    {
        // 初始化队列
        nextQueue.Clear();
        int typeCount = System.Enum.GetValues(typeof(TetrominoType)).Length;
        for (int i = 0; i < 2; i++)
            nextQueue.Enqueue((TetrominoType)Random.Range(0, typeCount));
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
            }
            else
            {
                PlaceToGrid();
                int cleared = grid.CheckAndClearRings(out int chain);
                if (cleared > 0)
                {
                    int[] comboBonus = {0, 0, 2, 4, 8};
                    int bonus = 0;
                    if (cleared < comboBonus.Length)
                        bonus = comboBonus[cleared];
                    else
                        bonus = comboBonus[4] + (cleared - 4) * 4;
                    score += cleared * 1 + bonus;
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
                    if (UIManager.Instance != null)
                    {
                        UIManager.Instance.ShowGameOver(score);
                        UIManager.Instance.ShowGameOver(score);
                    }
                    return;
                }
                SpawnNewTetromino();
            }
        }
        
        if (isActive && activeCells.Count > 0)
        {
            UpdateActiveCellsPosition();
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
        int layer = grid.layerCount - 1;
        int ring = this.ring;
        currentTetromino = new Tetromino(type, rotation, layer, ring);
        isActive = true;
        fallTimer = 0f;
        isFastFalling = false;
        CreateActiveCells();

        // 通知UI刷新
        updateNextQueue?.Invoke(new List<TetrominoType>(nextQueue));
    }

    void CreateActiveCells()
    {
        // 清理旧的
        foreach (var go in activeCells)
            Destroy(go);
        activeCells.Clear();
        int[,] shape = currentTetromino.GetShape();
        for (int y = 0; y < 3; y++)
        for (int x = 0; x < 3; x++)
        {
            if (shape[y, x] == 0) continue;
            var go = Instantiate(blockPrefab, transform);
            
            // 添加方块信息脚本
            var blockInfo = go.GetComponent<BlockInfo>();
            if (blockInfo == null)
            {
                blockInfo = go.AddComponent<BlockInfo>();
            }
            blockInfo.SetInfo(currentTetromino.type, currentTetromino.rotation, 
                            currentTetromino.layer, currentTetromino.ring + x, true);
            
            activeCells.Add(blockInfo);
        }
        UpdateActiveCellsPosition();
    }

    void UpdateActiveCellsPosition()
    {
        int[,] shape = currentTetromino.GetShape();
        int idx = 0;
        for (int y = 0; y < 3; y++)
        for (int x = 0; x < 3; x++)
        {
            if (shape[y, x] == 0) continue;
            int gridLayer = currentTetromino.layer + y;
            int gridRing = (currentTetromino.ring + x) % grid.ringCount;
            float angle = gridRing * 360f / grid.ringCount;
            float rad = angle * Mathf.Deg2Rad;
            float px = Mathf.Cos(rad) * grid.radius;
            float pz = Mathf.Sin(rad) * grid.radius;
            float py = gridLayer * grid.cellHeight;
            var go = activeCells[idx++];
            go.transform.position = new Vector3(px, py, pz);
            go.transform.LookAt(new Vector3(0, py, 0), Vector3.up);
            var blockInfo = go.GetComponent<BlockInfo>();
            if (blockInfo != null)
            {
                blockInfo.SetPosition(gridLayer, gridRing);
            }
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

    
    bool TryMove(int dRing)
    {
        int newRing = GetActualRingForPlacement();
        if (CanPlace(currentTetromino.layer, newRing,currentTetromino.rotation))
        {
            return true;
        }
        return false;
    }

    public bool TryRotate()
    {
        int newRot = (currentTetromino.rotation + 1) % 4;
        if (CanPlaceWithRotation(currentTetromino.layer, newRot))
        {
            currentTetromino.rotation = newRot;
            return true;
        }
        return false;
    }

    bool CanPlace(int layer, int ring, int rotation)
    {
        int[,] shape = TetrominoData.Shapes[currentTetromino.type][rotation];
        for (int y = 0; y < 3; y++)
        for (int x = 0; x < 3; x++)
        {
            if (shape[y, x] == 0) continue;
            int gridLayer = layer + y - 1;
            int gridRing = (ring + x) % grid.ringCount;
            if (gridLayer < 0 || gridLayer >= grid.layerCount) return false;
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

    public void SetFastFalling(bool fast)
    {
        isFastFalling = fast;
    }

    // 提供给UI获取队列
    public List<TetrominoType> GetNextQueue()
    {
        return new List<TetrominoType>(nextQueue);
    }
}

public static class CylindricalGridExtensions
{
    // 检查并清除满环，返回消除层数和连锁数
    public static int CheckAndClearRings(this CylindricalGrid grid, out int chain)
    {
        int cleared = 0;
        chain = 0;
        
        for (int y = 0; y < grid.layerCount; y++)
        {
            bool full = true;
            int filledCount = 0;
            for (int i = 0; i < grid.ringCount; i++)
            {
                if (grid.grid[y, i] == 0)
                {
                    full = false;
                }
                else
                {
                    filledCount++;
                }
            }
            
            if (full)
            {
                grid.ClearRing(y);
                cleared++;
                chain++;
                y--; // 检查新下落的这一层
            }
        }
        
        if (cleared > 0)
        {
            Debug.Log($"总共清除了{cleared}层，连锁数: {chain}");
        }
        else
        {
            Debug.Log("没有发现满环");
        }
        return cleared;
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
                    foreach (Transform child in cellObj.transform)
                    {
                        MonoBehaviour.Destroy(child.gameObject);
                    }
                }
            }
            grid.grid[layer, i] = 0;
        }
        // === 光圈特效 ===
        if (grid.ringClearEffectPrefab != null)
        {
            float yPos = layer * grid.cellHeight;
            GameObject fx = GameObject.Instantiate(grid.ringClearEffectPrefab, new Vector3(0, yPos, 0), Quaternion.identity);
            fx.transform.SetParent(grid.transform, false);
            fx.transform.localEulerAngles = new Vector3(90, 0, 0);
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
                // 将上层方块的子对象移动到当前层
                var children = new List<Transform>();
                foreach (Transform child in upperCell.transform)
                {
                    children.Add(child);
                }
                if (children.Count > 0)
                {
                    foreach (var child in children)
                    {
                        child.SetParent(currentCell.transform);
                        child.localPosition = Vector3.zero;
                        child.localRotation = Quaternion.identity;
                    }
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
                foreach (Transform child in topCell.transform)
                {
                    MonoBehaviour.Destroy(child.gameObject);
                }
            }
        }
    }
} 
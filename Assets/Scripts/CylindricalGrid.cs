using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CylindricalGrid : MonoBehaviour
{
    [Header("Grid Settings")]
    public int ringCount = 20; // 环向格子数
    public int layerCount = 10; // 高度层数
    public float radius = 5f;   // 圆柱半径
    public float cellHeight = 0.5f; // 每层高度
    public float cellWidth = 1f;    // 每格宽度（弧长）
    public float cellZ = 1f; 
    public GameObject cellPrefab; // 新增：格子预制体
    public Transform GridMeshsContent;

    public GameObject ringClearEffectPrefab;
    // 逻辑格子数据 0=空，1=有方块
    public int[,] grid;
    // 可视化格子
    public GameObject[,] cellObjects;
    private bool gridGenerated = false;
    private List<GameObject> cellPool = new List<GameObject>();

    void Awake()
    {
        grid = new int[layerCount, ringCount];
        cellObjects = new GameObject[layerCount, ringCount];
        StartCoroutine(GenerateGridMeshAsync());
    }

    IEnumerator GenerateGridMeshAsync()
    {
        // 回收旧格子到池
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
            cellPool.Add(child.gameObject);
        }

        int batch = 40; // 每帧生成多少个
        int count = 0;

        for (int y = 0; y < layerCount; y++)
        {
            for (int i = 0; i < ringCount; i++)
            {
                float angle = i * 360f / ringCount;
                float rad = angle * Mathf.Deg2Rad;
                float x = Mathf.Cos(rad) * radius;
                float z = Mathf.Sin(rad) * radius;
                float yPos = y * cellHeight;
                
                if (i == 9 || i == 10 || i == 11)
                {
                    GameObject cellMesh = null;
                    if (cellPool.Count > 0)
                    {
                        cellMesh = cellPool[cellPool.Count - 1];
                        cellPool.RemoveAt(cellPool.Count - 1);
                        cellMesh.SetActive(true);
                    }
                    else if (cellPrefab != null)
                    {
                        cellMesh = Instantiate(cellPrefab, GridMeshsContent);
                    }
                    else
                    {
                        cellMesh = GameObject.CreatePrimitive(PrimitiveType.Quad);
                    }
                    cellMesh.transform.SetParent(GridMeshsContent);
                    cellMesh.transform.localPosition = new Vector3(x, yPos, z);
                    cellMesh.transform.localScale = new Vector3(cellWidth, cellHeight, cellZ);
                    Vector3 centerMesh = new Vector3(0, yPos, 0);
                    cellMesh.transform.LookAt(centerMesh, Vector3.up);
                    cellMesh.transform.localEulerAngles = new Vector3(0, cellMesh.transform.localEulerAngles.y, 0);
                }

                GameObject cell  = new GameObject();
                cell.name = "gird";
                cell.transform.SetParent(transform);
                cell.transform.localPosition = new Vector3(x, yPos, z);
                cell.transform.localScale = new Vector3(cellWidth, cellHeight, cellZ);
                Vector3 center = new Vector3(0, yPos, 0);
                cell.transform.LookAt(center, Vector3.up);
                cell.transform.localEulerAngles = new Vector3(0, cell.transform.localEulerAngles.y, 0);

                var cellInfo = cell.GetComponent<GridCellInfo>();
                if (cellInfo == null)
                    cellInfo = cell.AddComponent<GridCellInfo>();
                cellInfo.SetInfo(y, i, false);

                cellObjects[y, i] = cell;

                count++;
                if (count % batch == 0)
                    yield return null; // 分帧
            }
        }
        gridGenerated = true;
    }
    
    public void SetCell(int layer, int ring, bool filled)
    {
        grid[layer, ring] = filled ? 1 : 0;
            
        // 更新网格信息
        var cellInfo = cellObjects[layer, ring].GetComponent<GridCellInfo>();
        if (cellInfo != null)
        {
            cellInfo.SetOccupied(filled);
        }
    }

    // 只重置，不销毁
    public void ClearGrid()
    {
        for (int y = 0; y < layerCount; y++)
        for (int i = 0; i < ringCount; i++)
        {
            SetCell(y, i, false);
            var cell = cellObjects[y, i];
            if (cell != null)
            {
                // 只清理子对象（方块），不销毁cell本身
                var children = new List<Transform>();
                foreach (Transform child in cell.transform)
                    children.Add(child);
                foreach (var child in children)
                    GameObject.Destroy(child.gameObject);
            }
        }
    }

    public void GenerateGridMesh() // 保留兼容接口，实际调用协程
    {
        StartCoroutine(GenerateGridMeshAsync());
    }
} 
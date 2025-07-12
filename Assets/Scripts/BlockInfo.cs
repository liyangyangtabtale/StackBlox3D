using UnityEngine;

public class BlockInfo : MonoBehaviour
{
    [Header("Block Info")]
    public TetrominoType blockType;
    public int rotation;
    public int layer;
    public int ring;
    public bool isActive = true;
    
    private Renderer blockRenderer;
    
    public void SetInfo(TetrominoType type, int rot, int l, int r, bool active)
    {
        blockType = type;
        rotation = rot;
        layer = l;
        ring = r;
        isActive = active;
        
        UpdateDisplay();
    }
    
    void UpdateDisplay()
    {
        blockRenderer = GetComponent<Renderer>();
        if (blockRenderer != null)
        {
            Color color = Color.white;
            switch (blockType)
            {
                case TetrominoType.Z: color = new Color(1f, 0f, 0f); break;         // 红
                case TetrominoType.L: color = new Color(1f, 0.5f, 0f); break;       // 橙
                case TetrominoType.O: color = new Color(1f, 1f, 0f); break;         // 黄
                case TetrominoType.S: color = new Color(0f, 1f, 0f); break;         // 绿
                case TetrominoType.I: color = new Color(0f, 1f, 1f); break;         // 青
                case TetrominoType.J: color = new Color(0f, 0f, 1f); break;         // 蓝
                case TetrominoType.T: color = new Color(0.6f, 0f, 1f); break;       // 紫
                case TetrominoType.B: color = new Color(1f, 0.4f, 0.7f); break;     // 粉（占位）
                case TetrominoType.V: color = new Color(0.2f, 0.8f, 0.6f); break;   // 青绿（占位）
                case TetrominoType.M: color = new Color(0.3f, 0.3f, 0.3f); break;   // 灰（占位）
            }
            blockRenderer.material.color = color;
        }
    }
    
    public void SetPosition(int l, int r)
    {
        layer = l;
        ring = r;
    }
    
    public void SetActive(bool active)
    {
        isActive = active;
    }
} 
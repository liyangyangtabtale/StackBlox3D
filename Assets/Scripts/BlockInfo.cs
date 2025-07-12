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
                case TetrominoType.O: color = new Color(1f, 0.851f, 0.4f, 1f); break;          // #ffd966ff 黄
                case TetrominoType.L: color = new Color(0.424f, 0.722f, 0.298f, 1f); break;    // #6cb84cff 绿
                case TetrominoType.J: color = new Color(0.302f, 0.522f, 0.973f, 1f); break;    // #4d85f8ff 蓝
                case TetrominoType.T: color = new Color(0.561f, 0.380f, 0.859f, 1f); break;    // #8f61dbff 紫
                case TetrominoType.S: color = new Color(0.039f, 0.694f, 0.671f, 1f); break;    // #0ab1abff 青绿
                case TetrominoType.Z: color = new Color(1f, 0.596f, 0.765f, 1f); break;        // #ff98c3ff 粉
                case TetrominoType.B: color = new Color(0.373f, 0.439f, 0.573f, 1f); break;    // #5f7092ff 深蓝灰
                case TetrominoType.I: color = new Color(1f, 0.6f, 0f, 1f); break;              // #ff9900ff 橙
                case TetrominoType.V: color = new Color(0.859f, 0.141f, 0.118f, 1f); break;    // #db241eff 红
                case TetrominoType.M: color = new Color(0.294f, 0.769f, 0.961f, 1f); break;    // #4bc4f5ff 浅蓝
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
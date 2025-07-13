using System.Collections.Generic;
using UnityEngine;

public class BlockInfo : MonoBehaviour
{
    [Header("Block Info")]
    public TetrominoType blockType;
    public int rotation;
    public int layer;
    public int ring;
    public bool isActive = true;
    
    [SerializeField] private Renderer blockRenderer;
    
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
        blockRenderer.sharedMaterials = BlockController.Instance.materials[blockType];
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
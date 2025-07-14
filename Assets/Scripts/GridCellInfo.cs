using UnityEngine;

public class GridCellInfo : MonoBehaviour
{
    [Header("Debug Info")]
    public int layerIndex;
    public int ringIndex;
    public bool isOccupied = false;
    public void SetInfo(int layer, int ring, bool occupied)
    {
        layerIndex = layer;
        ringIndex = ring;
        isOccupied = occupied;
    }
    
    public void SetOccupied(bool occupied)
    {
        isOccupied = occupied;
    }
} 
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class NextBlockPanel : MonoBehaviour
{
    public Image[] nextBlockImages; // Inspector拖入2个Image
    public Sprite[] blockTypeSprites; // 7种方块类型的Sprite

    void Start()
    {
        BlockController.updateNextQueue += UpdateNextBlocks;
        // 初始化显示
        var ctrl = FindObjectOfType<BlockController>();
        if (ctrl != null)
            UpdateNextBlocks(ctrl.GetNextQueue());
    }

    void OnDestroy()
    {
        BlockController.updateNextQueue -= UpdateNextBlocks;
    }

    public void UpdateNextBlocks(List<TetrominoType> queue)
    {
        for (int i = 0; i < nextBlockImages.Length; i++)
        {
            if (i < queue.Count)
                nextBlockImages[i].sprite = blockTypeSprites[(int)queue[i]];
            else
                nextBlockImages[i].sprite = null;
        }
    }
} 
using UnityEngine;
using UnityEditor;
using System.IO;

public class GenerateTetrominoSprites : EditorWindow
{
    [MenuItem("Tools/Generate Tetromino Sprites (10 Types)")]
    public static void GenerateSprites()
    {
        int size = 64;
        var types = System.Enum.GetValues(typeof(TetrominoType));
        Color[] colors = new Color[] {
            new Color(1f,1f,0f),    // O 黄
            new Color(1f,0.5f,0f),  // L 橙
            new Color(0f,0f,1f),    // J 蓝
            new Color(0.6f,0f,1f),  // T 紫
            new Color(0f,1f,0f),    // S 绿
            new Color(1f,0f,0f),    // Z 红
            new Color(1f,0.4f,0.7f),// B 粉
            new Color(0f,1f,1f),    // I 青
            new Color(0.2f,0.8f,0.6f),// V 青绿
            new Color(0.3f,0.3f,0.3f) // M 灰
        };
        string folder = "Assets/Sprites/";
        if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
        for (int t = 0; t < types.Length; t++)
        {
            Texture2D tex = new Texture2D(size, size);
            Color fill = colors[t % colors.Length];
            // 填充背景透明
            for (int y = 0; y < size; y++)
            for (int x = 0; x < size; x++)
                tex.SetPixel(x, y, new Color(0,0,0,0));
            // 画3x3格子
            int cell = size / 3;
            for (int by = 0; by < 3; by++)
            for (int bx = 0; bx < 3; bx++)
            {
                // 填充色块
                for (int y = by*cell+2; y < (by+1)*cell-2; y++)
                for (int x = bx*cell+2; x < (bx+1)*cell-2; x++)
                    tex.SetPixel(x, y, fill);
                // 黑色边框
                for (int i = 0; i < cell; i++)
                {
                    tex.SetPixel(bx*cell+i, by*cell, Color.black);
                    tex.SetPixel(bx*cell+i, (by+1)*cell-1, Color.black);
                    tex.SetPixel(bx*cell, by*cell+i, Color.black);
                    tex.SetPixel((bx+1)*cell-1, by*cell+i, Color.black);
                }
            }
            tex.Apply();
            byte[] png = tex.EncodeToPNG();
            string name = $"Tetromino_{types.GetValue(t)}.png";
            File.WriteAllBytes(folder + name, png);
            AssetDatabase.ImportAsset(folder + name);
        }
        AssetDatabase.Refresh();
        EditorUtility.DisplayDialog("生成完成", "10种方块占位Sprite已生成到Assets/Sprites/", "OK");
    }
} 
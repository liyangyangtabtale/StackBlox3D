using UnityEngine;
using UnityEditor;
using System.IO;

public class GenerateRingEffectTexture : EditorWindow
{
    [MenuItem("Tools/Generate Ring Effect Texture")]
    public static void GenerateTexture()
    {
        int size = 256;
        Texture2D tex = new Texture2D(size, size, TextureFormat.ARGB32, false);
        Color center = new Color(0.6f, 0f, 1f, 0f); // 紫色透明
        Color ring = new Color(0.6f, 0f, 1f, 0.7f); // 紫色高亮
        float r0 = 0.35f, r1 = 0.48f, r2 = 0.5f, r3 = 0.6f;
        Vector2 c = new Vector2(size/2f, size/2f);
        for (int y = 0; y < size; y++)
        for (int x = 0; x < size; x++)
        {
            float dist = Vector2.Distance(new Vector2(x, y), c) / (size/2f);
            Color col = center;
            if (dist > r0 && dist < r3)
            {
                float t = Mathf.InverseLerp(r1, r2, dist);
                col = Color.Lerp(ring, center, Mathf.Abs(t));
                col.a *= 1f - Mathf.Abs(t);
            }
            tex.SetPixel(x, y, col);
        }
        tex.Apply();
        string folder = "Assets/Sprites/";
        if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
        string path = folder + "RingEffect.png";
        File.WriteAllBytes(path, tex.EncodeToPNG());
        AssetDatabase.ImportAsset(path);
        AssetDatabase.Refresh();
        EditorUtility.DisplayDialog("生成完成", "紫色光圈贴图已生成到Assets/Sprites/RingEffect.png", "OK");
    }
} 
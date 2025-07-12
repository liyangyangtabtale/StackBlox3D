using UnityEngine;
using UnityEditor;
using System.IO;

public class GenerateRingEffectPrefab : EditorWindow
{
    [MenuItem("Tools/Generate Ring Effect Prefab")]
    public static void GeneratePrefab()
    {
        string spritePath = "Assets/Sprites/RingEffect.png";
        Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(spritePath);
        if (sprite == null)
        {
            EditorUtility.DisplayDialog("错误", "未找到光圈贴图，请先生成RingEffect.png", "OK");
            return;
        }
        GameObject go = new GameObject("RingClearEffect");
        var sr = go.AddComponent<SpriteRenderer>();
        sr.sprite = sprite;
        sr.sortingOrder = 100;
        sr.color = new Color(0.6f, 0f, 1f, 0.7f);
        go.AddComponent<RingClearEffect>();
        string folder = "Assets/Prefabs/";
        if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
        string prefabPath = folder + "RingClearEffect.prefab";
        PrefabUtility.SaveAsPrefabAsset(go, prefabPath);
        GameObject.DestroyImmediate(go);
        AssetDatabase.Refresh();
        EditorUtility.DisplayDialog("生成完成", "RingClearEffect特效Prefab已生成到Assets/Prefabs/", "OK");
    }
} 
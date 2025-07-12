using UnityEngine;
using UnityEngine.UI;

public class ButtonSoundEffect : MonoBehaviour
{
    [Header("音效设置")]
    public AudioClip customClickSound; // 可选的自定义点击音效
    public bool useCustomSound = false; // 是否使用自定义音效
    
    private Button button;
    
    void Awake()
    {
        // 获取Button组件
        button = GetComponent<Button>();
        if (button == null)
        {
            Debug.LogWarning($"ButtonSoundEffect: 在 {gameObject.name} 上没有找到Button组件！");
            return;
        }
        
        // 添加点击事件监听
        button.onClick.AddListener(PlayClickSound);
    }
    
    void PlayClickSound()
    {
        if (AudioManager.Instance != null)
        {
            if (useCustomSound && customClickSound != null)
            {
                // 使用自定义音效
                AudioManager.Instance.PlaySfx(customClickSound);
            }
            else
            {
                // 使用默认按钮点击音效
                AudioManager.Instance.PlayButtonClickSound();
            }
        }
    }
    
    void OnDestroy()
    {
        // 清理事件监听
        if (button != null)
        {
            button.onClick.RemoveListener(PlayClickSound);
        }
    }
} 
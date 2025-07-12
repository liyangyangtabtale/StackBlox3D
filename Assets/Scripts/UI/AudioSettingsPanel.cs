using UnityEngine;
using UnityEngine.UI;

public class AudioSettingsPanel : MonoBehaviour
{
    [Header("音乐设置")]
    public Toggle musicToggle;
    public Slider musicVolumeSlider;
    
    [Header("音效设置")]
    public Toggle sfxToggle;
    public Slider sfxVolumeSlider;
    
    [Header("按钮")]
    public Button closeButton;
    
    void Start()
    {
        InitializeSettings();
        
        // 设置事件监听
        if (musicToggle != null)
        {
            musicToggle.onValueChanged.AddListener(OnMusicToggleChanged);
        }
        
        if (musicVolumeSlider != null)
        {
            musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        }
        
        if (sfxToggle != null)
        {
            sfxToggle.onValueChanged.AddListener(OnSfxToggleChanged);
        }
        
        if (sfxVolumeSlider != null)
        {
            sfxVolumeSlider.onValueChanged.AddListener(OnSfxVolumeChanged);
        }
        
        if (closeButton != null)
        {
            closeButton.onClick.AddListener(OnCloseButtonClicked);
        }
    }
    
    void InitializeSettings()
    {
        if (AudioManager.Instance != null)
        {
            // 初始化音乐设置
            if (musicToggle != null)
            {
                musicToggle.isOn = AudioManager.Instance.IsMusicEnabled();
            }
            
            if (musicVolumeSlider != null)
            {
                musicVolumeSlider.value = AudioManager.Instance.GetMusicVolume();
            }
            
            // 初始化音效设置
            if (sfxToggle != null)
            {
                sfxToggle.isOn = AudioManager.Instance.IsSfxEnabled();
            }
            
            if (sfxVolumeSlider != null)
            {
                sfxVolumeSlider.value = AudioManager.Instance.GetSfxVolume();
            }
        }
    }
    
    void OnMusicToggleChanged(bool isOn)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.EnableMusic(isOn);
            AudioManager.Instance.SaveAudioSettings();
        }
    }
    
    void OnMusicVolumeChanged(float volume)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetMusicVolume(volume);
            AudioManager.Instance.SaveAudioSettings();
        }
    }
    
    void OnSfxToggleChanged(bool isOn)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.EnableSfx(isOn);
            AudioManager.Instance.SaveAudioSettings();
        }
    }
    
    void OnSfxVolumeChanged(float volume)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetSfxVolume(volume);
            AudioManager.Instance.SaveAudioSettings();
        }
    }
    
    void OnCloseButtonClicked()
    {
        // 关闭面板
        gameObject.SetActive(false);
    }
    
    void OnDestroy()
    {
        // 清理事件监听
        if (musicToggle != null)
        {
            musicToggle.onValueChanged.RemoveListener(OnMusicToggleChanged);
        }
        
        if (musicVolumeSlider != null)
        {
            musicVolumeSlider.onValueChanged.RemoveListener(OnMusicVolumeChanged);
        }
        
        if (sfxToggle != null)
        {
            sfxToggle.onValueChanged.RemoveListener(OnSfxToggleChanged);
        }
        
        if (sfxVolumeSlider != null)
        {
            sfxVolumeSlider.onValueChanged.RemoveListener(OnSfxVolumeChanged);
        }
        
        if (closeButton != null)
        {
            closeButton.onClick.RemoveListener(OnCloseButtonClicked);
        }
    }
} 
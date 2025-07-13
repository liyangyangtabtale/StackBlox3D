using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    
    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;
    
    [Header("Music")]
    public AudioClip backgroundMusic;
    public float musicVolume = 0.5f;
    public bool enableMusic = true;
    
    [Header("Sound Effects")]
    public AudioClip blockLandSound;
    public AudioClip lineClearSound;
    public AudioClip gameOverSound;
    public AudioClip buttonClickSound;
    public AudioClip downSpeedSound;
    public float sfxVolume = 0.7f;
    public bool enableSfx = true;
    
    void Awake()
    {
        // 单例模式
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        // 初始化AudioSource
        if (musicSource == null)
        {
            musicSource = gameObject.AddComponent<AudioSource>();
        }
        if (sfxSource == null)
        {
            sfxSource = gameObject.AddComponent<AudioSource>();
        }
        
        // 设置音乐AudioSource
        musicSource.clip = backgroundMusic;
        musicSource.loop = true;
        musicSource.volume = musicVolume;
        musicSource.playOnAwake = false;
        
        // 设置音效AudioSource
        sfxSource.loop = false;
        sfxSource.volume = sfxVolume;
        sfxSource.playOnAwake = false;
    }
    
    void Start()
    {
        // 加载音效设置
        LoadAudioSettings();
        
        // 开始播放背景音乐
        if (enableMusic && backgroundMusic != null)
        {
            PlayBackgroundMusic();
        }
    }
    
    // 播放背景音乐
    public void PlayBackgroundMusic()
    {
        if (enableMusic && musicSource != null && backgroundMusic != null)
        {
            musicSource.Play();
        }
    }
    
    // 停止背景音乐
    public void StopBackgroundMusic()
    {
        if (musicSource != null)
        {
            musicSource.Stop();
        }
    }
    
    // 暂停背景音乐
    public void PauseBackgroundMusic()
    {
        if (musicSource != null)
        {
            musicSource.Pause();
        }
    }
    
    // 恢复背景音乐
    public void ResumeBackgroundMusic()
    {
        if (musicSource != null)
        {
            musicSource.UnPause();
        }
    }
    
    // 播放方块落地音效
    public void PlayBlockLandSound()
    {
        PlaySfx(blockLandSound);
    }
    
    // 播放消除行音效
    public void PlayLineClearSound()
    {
        PlaySfx(lineClearSound);
    }
    
    // 播放游戏结束音效
    public void PlayGameOverSound()
    {
        PlaySfx(gameOverSound);
    }
    
    // 播放按钮点击音效
    public void PlayButtonClickSound()
    {
        PlaySfx(buttonClickSound);
    }
    
    public void PlayDownSpeed()
    {
        PlaySfx(downSpeedSound);
    }
    
    // 播放音效的通用方法
    public void PlaySfx(AudioClip clip)
    {
        if (enableSfx && sfxSource != null && clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }
    
    // 设置音乐音量
    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        if (musicSource != null)
        {
            musicSource.volume = musicVolume;
        }
    }
    
    // 设置音效音量
    public void SetSfxVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
        if (sfxSource != null)
        {
            sfxSource.volume = sfxVolume;
        }
    }
    
    // 开启/关闭音乐
    public void EnableMusic(bool enable)
    {
        enableMusic = enable;
        if (enable)
        {
            PlayBackgroundMusic();
        }
        else
        {
            StopBackgroundMusic();
        }
    }
    
    // 开启/关闭音效
    public void EnableSfx(bool enable)
    {
        enableSfx = enable;
    }
    
    // 获取音乐是否开启
    public bool IsMusicEnabled()
    {
        return enableMusic;
    }
    
    // 获取音效是否开启
    public bool IsSfxEnabled()
    {
        return enableSfx;
    }
    
    // 获取音乐音量
    public float GetMusicVolume()
    {
        return musicVolume;
    }
    
    // 获取音效音量
    public float GetSfxVolume()
    {
        return sfxVolume;
    }
    
    // 保存音效设置
    public void SaveAudioSettings()
    {
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        PlayerPrefs.SetFloat("SfxVolume", sfxVolume);
        PlayerPrefs.SetInt("EnableMusic", enableMusic ? 1 : 0);
        PlayerPrefs.SetInt("EnableSfx", enableSfx ? 1 : 0);
        PlayerPrefs.Save();
    }
    
    // 加载音效设置
    public void LoadAudioSettings()
    {
        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            SetMusicVolume(PlayerPrefs.GetFloat("MusicVolume"));
        }
        
        if (PlayerPrefs.HasKey("SfxVolume"))
        {
            SetSfxVolume(PlayerPrefs.GetFloat("SfxVolume"));
        }
        
        if (PlayerPrefs.HasKey("EnableMusic"))
        {
            EnableMusic(PlayerPrefs.GetInt("EnableMusic") == 1);
        }
        
        if (PlayerPrefs.HasKey("EnableSfx"))
        {
            EnableSfx(PlayerPrefs.GetInt("EnableSfx") == 1);
        }
    }
} 
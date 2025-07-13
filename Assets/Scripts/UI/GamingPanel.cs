using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class GamingPanel : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public Button rotateButton;
    public EventTrigger fallButton;
    public BlockController blockController;
    
    [Header("Line Clear Effects")]
    public Image lineClearEffectImage;
    public Sprite[] lineClearSprites; // 对应消除1行、2行、3行等的图片
    public float effectDuration = 1.5f;
    public float fadeInDuration = 0.3f;
    public float fadeOutDuration = 0.5f;

    void Start()
    {
        if (rotateButton != null)
        {
            rotateButton.onClick.AddListener(OnRotateClicked);
        }
        if (fallButton != null)
        {
            // 按下
            var entryDown = new EventTrigger.Entry { eventID = EventTriggerType.PointerDown };
            entryDown.callback.AddListener((data) => OnFallButtonDown());
            fallButton.triggers.Add(entryDown);
            // 松开
            var entryUp = new EventTrigger.Entry { eventID = EventTriggerType.PointerUp };
            entryUp.callback.AddListener((data) => OnFallButtonUp());
            fallButton.triggers.Add(entryUp);
        }
        BlockController.updateScore += UpdateScore;
        BlockController.onLineClear += ShowLineClearEffect;
        
        // 初始化消除行效果图片
        if (lineClearEffectImage != null)
        {
            lineClearEffectImage.gameObject.SetActive(false);
        }
    }

    void OnDestroy()
    {
        BlockController.updateScore -= UpdateScore;
        BlockController.onLineClear -= ShowLineClearEffect;
    }

    void UpdateScore(int score)
    {
        scoreText.text = $"{score}";
    }

    void OnRotateClicked()
    {
        if (blockController != null)
        {
            blockController.TryRotate();
        }
    }

    void OnFallButtonDown()
    {
        if (blockController != null)
        {
            blockController.SetFastFalling(true);
        }
        AudioManager.Instance.PlayDownSpeed();
    }

    void OnFallButtonUp()
    {
        if (blockController != null)
        {
            blockController.SetFastFalling(false);
        }
    }

    public void ShowLineClearEffect(int clearedLines)
    {
        if (lineClearEffectImage == null || lineClearSprites == null) return;
        
        // 根据消除的行数选择对应的图片
        if (clearedLines > 0 && clearedLines <= lineClearSprites.Length)
        {
            lineClearEffectImage.sprite = lineClearSprites[clearedLines - 1];
            lineClearEffectImage.SetNativeSize();
            StartCoroutine(LineClearEffectAnimation());
        }
    }

    private IEnumerator LineClearEffectAnimation()
    {
        if (lineClearEffectImage == null) yield break;
        
        // 显示图片
        lineClearEffectImage.gameObject.SetActive(true);
        
        // 淡入效果
        Color startColor = lineClearEffectImage.color;
        startColor.a = 0f;
        lineClearEffectImage.color = startColor;
        
        // 淡入动画
        float elapsedTime = 0f;
        while (elapsedTime < fadeInDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeInDuration);
            Color color = lineClearEffectImage.color;
            color.a = alpha;
            lineClearEffectImage.color = color;
            yield return null;
        }
        
        // 保持显示
        float holdTime = effectDuration - fadeInDuration - fadeOutDuration;
        if (holdTime > 0)
        {
            yield return new WaitForSeconds(holdTime);
        }
        
        // 淡出动画
        elapsedTime = 0f;
        while (elapsedTime < fadeOutDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeOutDuration);
            Color color = lineClearEffectImage.color;
            color.a = alpha;
            lineClearEffectImage.color = color;
            yield return null;
        }
        
        // 隐藏图片
        lineClearEffectImage.gameObject.SetActive(false);
    }
} 
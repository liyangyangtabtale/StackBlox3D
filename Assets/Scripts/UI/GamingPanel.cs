using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GamingPanel : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public Button rotateButton;
    public EventTrigger fallButton;
    public BlockController blockController;

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
    }

    void OnFallButtonUp()
    {
        if (blockController != null)
        {
            blockController.SetFastFalling(false);
        }
    }
} 
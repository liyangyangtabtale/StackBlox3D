using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class OverGamePanel : MonoBehaviour
{
    public TextMeshProUGUI finalScoreText;
    public Button restartButton;

    void Start()
    {
        if (restartButton != null)
        {
            restartButton.onClick.AddListener(OnRestartClicked);
        }
    }

    public void ShowFinalScore(int score)
    {
        if (finalScoreText != null)
        {
            finalScoreText.text = $"{score}";
        }
    }

    void OnRestartClicked()
    {
        UIManager.Instance.ShowGamingPanel();
        UIManager.Instance.blockController.RestartGame();
    }
} 
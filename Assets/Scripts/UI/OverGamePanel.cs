using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class OverGamePanel : MonoBehaviour
{
    public TextMeshProUGUI finalScoreText;
    public Button restartButton;
    public ParticleSystem EndParticleSystem;
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
        EndParticleSystem.Play();
    }

    void OnRestartClicked()
    {
        UIManager.Instance.ShowGamingPanel();
        UIManager.Instance.blockController.RestartGame();
    }
} 
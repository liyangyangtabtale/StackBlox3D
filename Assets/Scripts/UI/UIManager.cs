using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public BlockController blockController;
    public CylinderRotator cylinderRotator;
    // 三个Panel
    public GameObject startGamePanel;
    public GameObject gamingPanel;
    public GameObject overGamePanel;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        ShowStartPanel();
    }

    public void ShowStartPanel()
    {
        if (startGamePanel != null) startGamePanel.SetActive(true);
        if (gamingPanel != null) gamingPanel.SetActive(false);
        if (overGamePanel != null) overGamePanel.SetActive(false);
        if (cylinderRotator != null) cylinderRotator.canRotate = false;
    }

    public void ShowGamingPanel()
    {
        if (startGamePanel != null) startGamePanel.SetActive(false);
        if (gamingPanel != null) gamingPanel.SetActive(true);
        if (overGamePanel != null) overGamePanel.SetActive(false);
        if (cylinderRotator != null) cylinderRotator.canRotate = true;
    }

    public void ShowOverPanel()
    {
        if (startGamePanel != null) startGamePanel.SetActive(false);
        if (gamingPanel != null) gamingPanel.SetActive(false);
        if (overGamePanel != null) overGamePanel.SetActive(true);
        if (cylinderRotator != null) cylinderRotator.canRotate = false;
    }

    public void ShowGameOver(int finalScore)
    {
        ShowOverPanel();
        var overPanel = overGamePanel != null ? overGamePanel.GetComponent<OverGamePanel>() : null;
        if (overPanel != null)
        {
            overPanel.ShowFinalScore(finalScore);
        }
    }
} 
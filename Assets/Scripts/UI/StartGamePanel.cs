using UnityEngine;
using UnityEngine.UI;

public class StartGamePanel : MonoBehaviour
{
    public Button startButton;
    
    void Start()
    {
        if (startButton != null)
        {
            startButton.onClick.AddListener(OnStartClicked);
        }
    }

    void OnStartClicked()
    {
        UIManager.Instance.ShowGamingPanel();
        UIManager.Instance.blockController.StartGame();
    }
} 
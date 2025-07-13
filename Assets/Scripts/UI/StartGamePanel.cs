using UnityEngine;
using UnityEngine.UI;

public class StartGamePanel : MonoBehaviour
{
    public Button startButton;

    public Toggle MeshToggle;
    public GameObject mesh;
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

    public void SetMesh()
    {
        mesh.SetActive(MeshToggle.isOn);
    }
} 
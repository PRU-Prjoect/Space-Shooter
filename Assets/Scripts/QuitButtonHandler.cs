using UnityEngine;
using UnityEngine.UI;

public class QuitButtonHandler : MonoBehaviour
{
    public Button quitButton;

    void Awake()
    {
        // Nếu chưa gán, tự động tìm
        if (quitButton == null)
        {
            quitButton = GameObject.Find("QuitButton")?.GetComponent<Button>();
        }

        if (quitButton != null)
        {
            quitButton.onClick.AddListener(OnQuitClicked);
            Debug.Log("Quit button connected successfully!");
        }
        else
        {
            Debug.LogError("Quit Button not found! Please assign it in Inspector or check GameObject name.");
        }
    }

    void OnQuitClicked()
    {
        Debug.Log("Quit button clicked!");
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}

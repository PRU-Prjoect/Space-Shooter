using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InstructionManager : MonoBehaviour
{
    [Header("UI References")]
    public Button backButton;

    [Header("Scene Settings")]
    public string mainMenuSceneName = "Startgame";

    void Start()
    {
        SetupButton();
    }

    void SetupButton()
    {
        if (backButton != null)
        {
            backButton.onClick.AddListener(GoBackToMainMenu);
        }
        else
        {
            Debug.LogError("Back Button not assigned!");
        }
    }

    public void GoBackToMainMenu()
    {
        Debug.Log("Returning to main menu from instructions...");
        SceneManager.LoadScene(mainMenuSceneName);
    }
}

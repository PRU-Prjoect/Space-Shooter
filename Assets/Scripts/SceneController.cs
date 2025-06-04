using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [Header("Scene Names")]
    public string startSceneName = "StartScene";
    public string gameSceneName = "GameScene";
    public string endSceneName = "EndScene";

    // Singleton pattern để dễ truy cập
    public static SceneController Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Giữ object khi chuyển scene
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Chuyển từ Start Scene → Game Scene
    public void StartGame()
    {
        Debug.Log("Starting game...");
        SceneManager.LoadScene(gameSceneName);
    }

    // Chuyển từ Game Scene → End Scene (khi thua)
    public void GameOver()
    {
        Debug.Log("Game Over! Loading End Scene...");
        SceneManager.LoadScene(endSceneName);
    }

    // Chuyển từ End Scene → Start Scene (chơi lại)
    public void RestartGame()
    {
        Debug.Log("Restarting game...");
        SceneManager.LoadScene(startSceneName);
    }

    // Thoát game
    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}

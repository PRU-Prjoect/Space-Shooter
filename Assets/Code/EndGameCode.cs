using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGameCode : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Button MainMenuButton;
    [SerializeField] private Button PlayAgainButton;
    [SerializeField] private TextMeshProUGUI ScoreEndGame;
    [SerializeField] private TextMeshProUGUI HighScoreText;

    [Header("Scene Names")]
    [SerializeField] private string mainMenuSceneName = "Startgame";
    [SerializeField] private string gameSceneName = "PlayScene";

    private void Awake()
    {
        SetupButtons();
        DisplayScores();
    }

    private void Start()
    {
        Time.timeScale = 1f; // Đảm bảo time scale về bình thường
    }

    void SetupButtons()
    {
        if (MainMenuButton != null)
        {
            MainMenuButton.onClick.AddListener(ReturnToMainMenu);
        }

        if (PlayAgainButton != null)
        {
            PlayAgainButton.onClick.AddListener(RetryGame);
        }
    }

    public void RetryGame()
    {
        Debug.Log("Restarting game...");
        SceneManager.LoadScene(gameSceneName);
    }

    public void ReturnToMainMenu()
    {
        Debug.Log("Returning to main menu...");
        SceneManager.LoadScene(mainMenuSceneName);
    }

    public void DisplayScores()
    {
        // Lấy điểm từ PlayerPrefs
        int finalScore = PlayerPrefs.GetInt("FinalScore", 0);
        int highScore = PlayerPrefs.GetInt("HighScore", 0);

        // Hiển thị điểm cuối game
        if (ScoreEndGame != null)
        {
            ScoreEndGame.text = $"Final Score: {finalScore:N0}";
        }

        // Hiển thị high score
        if (HighScoreText != null)
        {
            if (finalScore >= highScore && finalScore > 0)
            {
                HighScoreText.text = $"NEW HIGH SCORE!\n{highScore:N0}";
                HighScoreText.color = Color.yellow;
            }
            else
            {
                HighScoreText.text = $"Highest Score: {highScore:N0}";
                HighScoreText.color = Color.white;
            }
        }

        Debug.Log($"End Game - Final Score: {finalScore}, High Score: {highScore}");
    }

    // Hàm để các script khác gọi khi game over
    public static void TriggerGameOver(int finalScore)
    {
        // Lưu điểm cuối game
        PlayerPrefs.SetInt("FinalScore", finalScore);

        // Kiểm tra và cập nhật high score
        int currentHighScore = PlayerPrefs.GetInt("HighScore", 0);
        if (finalScore > currentHighScore)
        {
            PlayerPrefs.SetInt("HighScore", finalScore);
            Debug.Log($"New High Score: {finalScore}");
        }

        PlayerPrefs.Save();

        // Chuyển đến End Scene
        SceneManager.LoadScene("EndScene");
    }
}

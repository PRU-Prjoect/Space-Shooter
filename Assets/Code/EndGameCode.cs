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

    [Header("Audio")]
    [SerializeField] private AudioSource buttonClickSound;

    private void Awake()
    {
        SetupButtons();
        DisplayScores();
    }

    private void Start()
    {
        // Đảm bảo time scale về bình thường
        Time.timeScale = 1f;
    }

    void SetupButtons()
    {
        if (MainMenuButton != null)
        {
            MainMenuButton.onClick.AddListener(() =>
            {
                PlayButtonSound();
                ReturnToMainMenu();
            });
        }

        if (PlayAgainButton != null)
        {
            PlayAgainButton.onClick.AddListener(() =>
            {
                PlayButtonSound();
                RetryGame();
            });
        }
    }

    public void RetryGame()
    {
        Debug.Log("Restarting game...");

        // Reset PlayerPrefs nếu cần
        PlayerPrefs.SetInt("CurrentScore", 0);

        // Load game scene
        SceneManager.LoadScene(gameSceneName);
    }

    public void ReturnToMainMenu()
    {
        Debug.Log("Returning to main menu...");

        // Load main menu scene
        SceneManager.LoadScene(mainMenuSceneName);
    }

    public void DisplayScores()
    {
        // Lấy điểm từ PlayerPrefs
        int finalScore = PlayerPrefs.GetInt("FinalScore", 0);
        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        int previousHighScore = PlayerPrefs.GetInt("PreviousHighScore", 0);

        // Hiển thị điểm cuối game
        if (ScoreEndGame != null)
        {
            ScoreEndGame.text = $"Final Score: {finalScore:N0}";
        }

        // Hiển thị high score
        if (HighScoreText != null)
        {
            if (finalScore > previousHighScore && finalScore == highScore)
            {
                HighScoreText.text = $"NEW HIGH SCORE!\n{highScore:N0}";
                HighScoreText.color = Color.yellow;
            }
            else
            {
                HighScoreText.text = $"High Score: {highScore:N0}";
                HighScoreText.color = Color.white;
            }
        }

        Debug.Log($"End Game - Final Score: {finalScore}, High Score: {highScore}");
    }

    void PlayButtonSound()
    {
        if (buttonClickSound != null)
        {
            buttonClickSound.Play();
        }
    }

    // Hàm để các script khác gọi khi game over
    public static void TriggerGameOver(int finalScore)
    {
        // Lưu điểm cuối game
        PlayerPrefs.SetInt("FinalScore", finalScore);

        // Kiểm tra và cập nhật high score
        int currentHighScore = PlayerPrefs.GetInt("HighScore", 0);
        PlayerPrefs.SetInt("PreviousHighScore", currentHighScore);

        if (finalScore > currentHighScore)
        {
            PlayerPrefs.SetInt("HighScore", finalScore);
            Debug.Log($"New High Score: {finalScore}");
        }

        PlayerPrefs.Save();

        // Chuyển đến End Scene
        SceneManager.LoadScene("EndScene");
    }

    // Hàm tiện ích để format số
    private string FormatScore(int score)
    {
        if (score >= 1000000)
            return (score / 1000000f).ToString("F1") + "M";
        else if (score >= 1000)
            return (score / 1000f).ToString("F1") + "K";
        else
            return score.ToString();
    }

    // Hàm để restart với hiệu ứng fade (optional)
    public void RetryGameWithFade()
    {
        StartCoroutine(FadeAndLoadScene(gameSceneName));
    }

    public void ReturnToMainMenuWithFade()
    {
        StartCoroutine(FadeAndLoadScene(mainMenuSceneName));
    }

    private System.Collections.IEnumerator FadeAndLoadScene(string sceneName)
    {
        // Thêm hiệu ứng fade nếu có
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(sceneName);
    }
}

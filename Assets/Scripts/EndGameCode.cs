using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

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
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip buttonClickSound;

    private void Awake()
    {
        CheckUIReferences();
        SetupAudio();
        SetupButtons();
    }

    private void Start()
    {
        Time.timeScale = 1f;

        // Delay để đảm bảo UI đã sẵn sàng
        StartCoroutine(DisplayScoresWithDelay());
    }

    void CheckUIReferences()
    {
        Debug.Log($"=== UI REFERENCES CHECK ===");
        Debug.Log($"ScoreEndGame assigned: {ScoreEndGame != null}");
        Debug.Log($"HighScoreText assigned: {HighScoreText != null}");
        Debug.Log($"MainMenuButton assigned: {MainMenuButton != null}");
        Debug.Log($"PlayAgainButton assigned: {PlayAgainButton != null}");

        if (ScoreEndGame == null)
            Debug.LogError("ScoreEndGame not assigned in Inspector!");
        if (HighScoreText == null)
            Debug.LogError("HighScoreText not assigned in Inspector!");
    }

    void SetupAudio()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
        }

        audioSource.playOnAwake = false;
        audioSource.volume = 1.0f;
        audioSource.mute = false;

        Debug.Log("EndGame AudioSource setup completed");
    }

    void SetupButtons()
    {
        if (MainMenuButton != null)
        {
            MainMenuButton.onClick.RemoveAllListeners(); // Clear existing listeners
            MainMenuButton.onClick.AddListener(ReturnToMainMenu);
            Debug.Log("Main Menu button connected");
        }

        if (PlayAgainButton != null)
        {
            PlayAgainButton.onClick.RemoveAllListeners(); // Clear existing listeners
            PlayAgainButton.onClick.AddListener(RetryGame);
            Debug.Log("Play Again button connected");
        }
    }

    // Delay DisplayScores để đảm bảo UI ready
    IEnumerator DisplayScoresWithDelay()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(0.1f);
        DisplayScores();
    }

    public void DisplayScores()
    {
        Debug.Log("=== DISPLAY SCORES START ===");

        // Lấy điểm từ PlayerPrefs
        int finalScore = PlayerPrefs.GetInt("FinalScore", 0);
        int highScore = PlayerPrefs.GetInt("HighScore", 0);

        Debug.Log($"FinalScore from PlayerPrefs: {finalScore}");
        Debug.Log($"HighScore from PlayerPrefs: {highScore}");

        // Hiển thị Final Score
        UpdateFinalScoreUI(finalScore);

        // Hiển thị High Score với delay
        StartCoroutine(UpdateHighScoreUI(finalScore, highScore));

        Debug.Log("=== DISPLAY SCORES END ===");
    }

    void UpdateFinalScoreUI(int finalScore)
    {
        if (ScoreEndGame != null)
        {
            string finalScoreText = $"Final Score: {finalScore:N0}";
            ScoreEndGame.text = finalScoreText;
            Debug.Log($"Final Score UI updated: {finalScoreText}");

            // Force UI update
            ScoreEndGame.ForceMeshUpdate();
        }
        else
        {
            Debug.LogError("ScoreEndGame is NULL! Cannot update Final Score UI.");
        }
    }

    IEnumerator UpdateHighScoreUI(int finalScore, int highScore)
    {
        yield return new WaitForEndOfFrame();

        if (HighScoreText != null)
        {
            string highScoreText;
            Color textColor;

            if (finalScore > highScore && finalScore > 0)
            {
                // Có kỷ lục mới
                highScoreText = $"NEW HIGH SCORE!\n{finalScore:N0}";
                textColor = Color.yellow;
                Debug.Log($"NEW HIGH SCORE displayed: {finalScore}");
            }
            else
            {
                // Không có kỷ lục mới
                highScoreText = $"Highest Score: {highScore:N0}";
                textColor = Color.white;
                Debug.Log($"Regular high score displayed: {highScore}");
            }

            HighScoreText.text = highScoreText;
            HighScoreText.color = textColor;
            HighScoreText.ForceMeshUpdate();

            Debug.Log($"High Score UI updated: {highScoreText}");
        }
        else
        {
            Debug.LogError("HighScoreText is NULL! Cannot update High Score UI.");
        }

        // Force Canvas update
        yield return new WaitForEndOfFrame();
        Canvas.ForceUpdateCanvases();
    }

    public void RetryGame()
    {
        Debug.Log("Restarting game...");
        PlayButtonSound();

        // Chuyển về game music nếu có
        if (BackgroundMusicManager.Instance != null)
        {
            BackgroundMusicManager.Instance.PlayGameMusic();
        }

        StartCoroutine(LoadSceneWithDelay(gameSceneName, 0.2f));
    }

    public void ReturnToMainMenu()
    {
        Debug.Log("Returning to main menu...");
        PlayButtonSound();

        // Chuyển về menu music nếu có
        if (BackgroundMusicManager.Instance != null)
        {
            BackgroundMusicManager.Instance.PlayMenuMusic();
        }

        StartCoroutine(LoadSceneWithDelay(mainMenuSceneName, 0.2f));
    }

    void PlayButtonSound()
    {
        if (buttonClickSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(buttonClickSound, 1.0f);
            Debug.Log("Button sound played");
        }
        else
        {
            if (buttonClickSound == null)
                Debug.LogWarning("Button Click Sound not assigned!");
            if (audioSource == null)
                Debug.LogWarning("AudioSource is null!");
        }
    }

    IEnumerator LoadSceneWithDelay(string sceneName, float delay)
    {
        yield return new WaitForSeconds(delay);
        Debug.Log($"Loading scene: {sceneName}");
        SceneManager.LoadScene(sceneName);
    }

    // Hàm để các script khác gọi khi game over
    public static void TriggerGameOver(int finalScore)
    {
        Debug.Log($"=== TRIGGER GAME OVER ===");
        Debug.Log($"Final Score received: {finalScore}");

        // Lưu điểm cuối game
        PlayerPrefs.SetInt("FinalScore", finalScore);

        // Kiểm tra và cập nhật high score
        int currentHighScore = PlayerPrefs.GetInt("HighScore", 0);
        Debug.Log($"Current High Score: {currentHighScore}");

        if (finalScore > currentHighScore)
        {
            PlayerPrefs.SetInt("HighScore", finalScore);
            Debug.Log($"New High Score saved: {finalScore}");
        }
        else
        {
            Debug.Log($"No new high score. Final: {finalScore}, Current: {currentHighScore}");
        }

        PlayerPrefs.Save();
        Debug.Log("PlayerPrefs saved");

        // Chuyển đến End Scene
        SceneManager.LoadScene("EndScene");
    }

    // Debug functions
    [ContextMenu("Force Refresh UI")]
    void ForceRefreshUI()
    {
        DisplayScores();
    }

    [ContextMenu("Test UI with Sample Data")]
    void TestUIWithSampleData()
    {
        PlayerPrefs.SetInt("FinalScore", 1500);
        PlayerPrefs.SetInt("HighScore", 1000);
        DisplayScores();
    }

    [ContextMenu("Debug PlayerPrefs")]
    void DebugPlayerPrefs()
    {
        Debug.Log($"FinalScore: {PlayerPrefs.GetInt("FinalScore", -1)}");
        Debug.Log($"HighScore: {PlayerPrefs.GetInt("HighScore", -1)}");
    }

    [ContextMenu("Reset All Scores")]
    void ResetAllScores()
    {
        PlayerPrefs.DeleteKey("FinalScore");
        PlayerPrefs.DeleteKey("HighScore");
        PlayerPrefs.Save();
        Debug.Log("All scores reset!");
        DisplayScores();
    }
}

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
    [SerializeField] private AudioClip gameOverMusic;

    private void Awake()
    {
        SetupAudio(); // SỬA: Setup audio trước
        SetupButtons();
        DisplayScores();
    }

    private void Start()
    {
        Time.timeScale = 1f; // Đảm bảo time scale về bình thường
        PlayGameOverMusic(); // SỬA: Phát nhạc game over
    }

    void SetupAudio()
    {
        // SỬA: Tự động tạo AudioSource nếu chưa có
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
        }

        // SỬA: Cấu hình AudioSource đúng
        audioSource.playOnAwake = false;
        audioSource.volume = 1.0f;
        audioSource.mute = false;

        Debug.Log("EndGame AudioSource setup completed");
    }

    void PlayGameOverMusic()
    {
        // SỬA: Phát nhạc game over hoặc dừng music
        if (BackgroundMusicManager.Instance != null)
        {
            if (gameOverMusic != null)
            {
                BackgroundMusicManager.Instance.musicSource.clip = gameOverMusic;
                BackgroundMusicManager.Instance.musicSource.Play();
                Debug.Log("Game Over music started");
            }
            else
            {
                BackgroundMusicManager.Instance.StopMusic();
                Debug.Log("Background music stopped for game over");
            }
        }
    }

    void SetupButtons()
    {
        if (MainMenuButton != null)
        {
            MainMenuButton.onClick.AddListener(ReturnToMainMenu);
            Debug.Log("Main Menu button connected");
        }

        if (PlayAgainButton != null)
        {
            PlayAgainButton.onClick.AddListener(RetryGame);
            Debug.Log("Play Again button connected");
        }
    }

    public void RetryGame()
    {
        Debug.Log("Restarting game...");

        // SỬA: Phát âm thanh và delay chuyển scene
        PlayButtonSound();

        // Chuyển về game music
        if (BackgroundMusicManager.Instance != null)
        {
            BackgroundMusicManager.Instance.PlayGameMusic();
        }

        StartCoroutine(LoadSceneWithDelay(gameSceneName, 0.2f));
    }

    public void ReturnToMainMenu()
    {
        Debug.Log("Returning to main menu...");

        // SỬA: Phát âm thanh và delay chuyển scene
        PlayButtonSound();

        // Chuyển về menu music
        if (BackgroundMusicManager.Instance != null)
        {
            BackgroundMusicManager.Instance.PlayMenuMusic();
        }

        StartCoroutine(LoadSceneWithDelay(mainMenuSceneName, 0.2f));
    }

    void PlayButtonSound()
    {
        // SỬA: Thêm debug để kiểm tra
        Debug.Log($"Playing end game button sound. AudioSource: {audioSource != null}, Sound: {buttonClickSound != null}");

        if (buttonClickSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(buttonClickSound, 1.0f);
            Debug.Log("End game button sound played!");
        }
        else
        {
            if (buttonClickSound == null)
                Debug.LogError("Button Click Sound is NULL! Please assign it in Inspector.");
            if (audioSource == null)
                Debug.LogError("AudioSource is NULL!");
        }
    }

    // SỬA: Thêm coroutine để delay chuyển scene
    IEnumerator LoadSceneWithDelay(string sceneName, float delay)
    {
        // Đợi âm thanh phát xong
        yield return new WaitForSeconds(delay);

        Debug.Log($"Loading scene: {sceneName}");
        SceneManager.LoadScene(sceneName);
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

                // SỬA: Thêm hiệu ứng nhấp nháy cho NEW HIGH SCORE
                StartCoroutine(BlinkHighScoreText());
            }
            else
            {
                HighScoreText.text = $"Highest Score: {highScore:N0}";
                HighScoreText.color = Color.white;
            }
        }

        Debug.Log($"End Game - Final Score: {finalScore}, High Score: {highScore}");
    }

    // SỬA: Hiệu ứng nhấp nháy cho NEW HIGH SCORE
    IEnumerator BlinkHighScoreText()
    {
        for (int i = 0; i < 6; i++)
        {
            HighScoreText.color = Color.yellow;
            yield return new WaitForSeconds(0.3f);
            HighScoreText.color = Color.red;
            yield return new WaitForSeconds(0.3f);
        }
        HighScoreText.color = Color.yellow; // Kết thúc bằng màu vàng
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

    // SỬA: Thêm hàm test
    [ContextMenu("Test End Game Button Sound")]
    void TestEndGameButtonSound()
    {
        PlayButtonSound();
    }
}

using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [Header("Score Settings")]
    public int currentScore = 0;
    public int highScore = 0;

    [Header("Score Values")]
    public int scorePerObstacle = 10;
    public int scorePerSecond = 1;
    public int bonusScore = 50;

    [Header("Background Change")]
    public int scoreToChangeBackground = 20;
    private int lastBackgroundChangeScore = 0;

    [Header("UI References")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;

    [Header("Game Events")]
    public static System.Action<int> OnScoreChanged; // Event cho score thay đổi
    public static System.Action<int> OnHighScoreChanged; // Event cho high score mới

    private float timeCounter = 0f;

    void Start()
    {
        LoadHighScore();
        UpdateScoreUI();
    }

    void Update()
    {
        timeCounter += Time.deltaTime;
        if (timeCounter >= 1f)
        {
            AddScore(scorePerSecond);
            timeCounter = 0f;
        }
    }

    public void AddScore(int points)
    {
        currentScore += points;
        UpdateScoreUI();

        // Trigger event khi score thay đổi
        OnScoreChanged?.Invoke(currentScore);

        CheckBackgroundChange();

        // Kiểm tra high score
        if (currentScore > highScore)
        {
            int oldHighScore = highScore;
            highScore = currentScore;
            SaveHighScore();

            // Trigger event khi có high score mới
            OnHighScoreChanged?.Invoke(highScore);
            Debug.Log($"NEW HIGH SCORE! {oldHighScore} → {highScore}");
        }
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = $"Score: {currentScore:N0}"; // Format với dấu phẩy

        if (highScoreText != null)
            highScoreText.text = $"Highest Score: {highScore:N0}"; // Đổi thành "Best" cho ngắn gọn
    }

    void SaveHighScore()
    {
        PlayerPrefs.SetInt("HighScore", highScore);
        PlayerPrefs.Save();
    }

    void LoadHighScore()
    {
        highScore = PlayerPrefs.GetInt("HighScore", 0);
    }

    public void ResetScore()
    {
        currentScore = 0;
        lastBackgroundChangeScore = 0;
        UpdateScoreUI();
        OnScoreChanged?.Invoke(currentScore);
    }

    void CheckBackgroundChange()
    {
        if (currentScore - lastBackgroundChangeScore >= scoreToChangeBackground)
        {
            ChangeBackground();
            lastBackgroundChangeScore = currentScore;
        }
    }

    void ChangeBackground()
    {
        Background background = Object.FindFirstObjectByType<Background>();
        if (background != null)
        {
            background.NextBackground();
            Debug.Log($"Background changed at score: {currentScore}");

            // Refill ammo khi chuyển background
            PlayerBehaviour player = Object.FindFirstObjectByType<PlayerBehaviour>();
            if (player != null)
            {
                player.OnBackgroundChanged();
            }
        }
    }

    public void GameOver()
    {
        // Lưu điểm cuối game
        PlayerPrefs.SetInt("FinalScore", currentScore);

        // Cập nhật high score nếu cần
        if (currentScore > highScore)
        {
            highScore = currentScore;
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();
        }

        Debug.Log($"Game Over! Final Score: {currentScore}, High Score: {highScore}");
    }

    // Hàm tiện ích để các script khác sử dụng
    public int GetCurrentScore() => currentScore;
    public int GetHighScore() => highScore;

    // Hàm để set score (cho testing hoặc cheat)
    public void SetScore(int score)
    {
        currentScore = score;
        UpdateScoreUI();
        OnScoreChanged?.Invoke(currentScore);
    }

    // Cleanup events khi destroy
    void OnDestroy()
    {
        OnScoreChanged = null;
        OnHighScoreChanged = null;
    }
}

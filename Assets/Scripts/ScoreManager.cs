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

    [Header("Background Change")] // THÊM MỚI - không ảnh hưởng tính năng cũ
    public int scoreToChangeBackground = 20;
    private int lastBackgroundChangeScore = 0;

    [Header("UI References")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;

    private float timeCounter = 0f;

    void Start()
    {
        LoadHighScore();
        UpdateScoreUI();
    }

    void Update()
    {
        // GIỮ NGUYÊN - Tính điểm theo thời gian
        timeCounter += Time.deltaTime;
        if (timeCounter >= 1f)
        {
            AddScore(scorePerSecond);
            timeCounter = 0f;
        }
    }

    public void AddScore(int points)
    {
        // GIỮ NGUYÊN - Logic cộng điểm cũ
        currentScore += points;
        UpdateScoreUI();

        // THÊM MỚI - Chỉ thêm kiểm tra chuyển background
        CheckBackgroundChange();

        // GIỮ NGUYÊN - Kiểm tra high score
        if (currentScore > highScore)
        {
            highScore = currentScore;
            SaveHighScore();
        }
    }

    void UpdateScoreUI()
    {
        // GIỮ NGUYÊN - Hiển thị UI như cũ
        if (scoreText != null)
            scoreText.text = "Score: " + currentScore.ToString();

        if (highScoreText != null)
            highScoreText.text = "High Score: " + highScore.ToString();
    }

    void SaveHighScore()
    {
        // GIỮ NGUYÊN - Lưu high score
        PlayerPrefs.SetInt("HighScore", highScore);
        PlayerPrefs.Save();
    }

    void LoadHighScore()
    {
        // GIỮ NGUYÊN - Load high score
        highScore = PlayerPrefs.GetInt("HighScore", 0);
    }

    public void ResetScore()
    {
        // GIỮ NGUYÊN + THÊM MỚI
        currentScore = 0;
        lastBackgroundChangeScore = 0; // Chỉ thêm dòng này
        UpdateScoreUI();
    }

    // THÊM MỚI - Các hàm chuyển background
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
        }
    }
}

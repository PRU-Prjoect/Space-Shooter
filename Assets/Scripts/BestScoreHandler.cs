using UnityEngine;
using TMPro;

public class BestScoreHandle : MonoBehaviour
{
    public TMP_Text bestScoreText; // Kéo thả TextMeshPro Text vào đây trong Inspector
    private int bestScore = 0;

    void Start()
    {
        // Lấy giá trị best score đã lưu, nếu chưa có thì trả về 0
        bestScore = PlayerPrefs.GetInt("BestScore", 0);
        GetBestScoreText();
    }

    // Gọi hàm này mỗi khi bạn muốn cập nhật best score (ví dụ khi kết thúc game)
    public void CheckForBestScore(int currentScore)
    {
        if (currentScore > bestScore)
        {
            bestScore = currentScore;
            PlayerPrefs.SetInt("BestScore", bestScore);
            PlayerPrefs.Save();
            GetBestScoreText();
        }
    }

    private void GetBestScoreText()
    {
        bestScoreText.text = "Best Score: " + bestScore.ToString();
    }
}

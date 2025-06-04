using UnityEngine;
using TMPro;
using System.Collections;

public class StartSceneUI : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI bestScoreText;

    [Header("Animation Settings")]
    public float animationDuration = 1f;

    private int bestScore = 0;

    void Start()
    {
        LoadAndDisplayBestScore();
        AnimateBestScore();
    }

    void LoadAndDisplayBestScore()
    {
        bestScore = PlayerPrefs.GetInt("HighScore", 0);

        if (bestScoreText != null)
        {
            if (bestScore > 0)
            {
                bestScoreText.text = $"Best Score: {bestScore:N0}";
            }
            else
            {
                bestScoreText.text = "Best Score: ---";
            }
        }
    }

    void AnimateBestScore()
    {
        if (bestScore > 0)
        {
            StartCoroutine(CountUpAnimation());
        }
    }

    IEnumerator CountUpAnimation()
    {
        int displayScore = 0;
        float elapsed = 0f;

        while (elapsed < animationDuration)
        {
            elapsed += Time.deltaTime;
            float progress = elapsed / animationDuration;

            displayScore = Mathf.RoundToInt(Mathf.Lerp(0, bestScore, progress));
            bestScoreText.text = $"Best Score: {displayScore:N0}";

            yield return null;
        }

        bestScoreText.text = $"Best Score: {bestScore:N0}";
    }

    // Hàm để test (có thể xóa)
    [ContextMenu("Test Refresh Best Score")]
    void TestRefresh()
    {
        LoadAndDisplayBestScore();
    }
}

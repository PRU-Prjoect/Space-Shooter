using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class InstructionManager : MonoBehaviour
{
    [Header("UI References")]
    public Button backButton;

    [Header("Scene Settings")]
    public string mainMenuSceneName = "Startgame";

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip buttonClickSound;

    void Start()
    {
        SetupAudio(); // SỬA: Setup audio trước
        SetupButton();
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

        Debug.Log("Instruction Manager AudioSource setup completed");
    }

    void SetupButton()
    {
        if (backButton != null)
        {
            backButton.onClick.AddListener(GoBackToMainMenu);
            Debug.Log("Back button connected successfully!");
        }
        else
        {
            Debug.LogError("Back Button not assigned!");
        }
    }

    public void GoBackToMainMenu()
    {
        Debug.Log("Returning to main menu from instructions...");

        // SỬA: Phát âm thanh và delay chuyển scene
        PlayButtonSound();

        // Delay để âm thanh kịp phát
        StartCoroutine(LoadSceneWithDelay(mainMenuSceneName, 0.2f));
    }

    void PlayButtonSound()
    {
        // SỬA: Thêm debug để kiểm tra
        Debug.Log($"Playing back button sound. AudioSource: {audioSource != null}, Sound: {buttonClickSound != null}");

        if (buttonClickSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(buttonClickSound, 1.0f);
            Debug.Log("Back button sound played!");
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
        // Chuyển về menu music nếu có BackgroundMusicManager
        if (BackgroundMusicManager.Instance != null)
        {
            BackgroundMusicManager.Instance.PlayMenuMusic();
        }

        // Đợi âm thanh phát xong
        yield return new WaitForSeconds(delay);

        Debug.Log($"Loading scene: {sceneName}");
        SceneManager.LoadScene(sceneName);
    }

    // SỬA: Thêm hàm test
    [ContextMenu("Test Back Button Sound")]
    void TestBackButtonSound()
    {
        PlayButtonSound();
    }
}

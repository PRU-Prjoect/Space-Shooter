using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class QuitButtonHandler : MonoBehaviour
{
    public Button quitButton;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip buttonClickSound;

    void Awake()
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

        Debug.Log("Quit button AudioSource setup completed");
    }

    void SetupButton()
    {
        // Nếu chưa gán, tự động tìm
        if (quitButton == null)
        {
            quitButton = GameObject.Find("QuitButton")?.GetComponent<Button>();
        }

        if (quitButton != null)
        {
            quitButton.onClick.AddListener(OnQuitClicked);
            Debug.Log("Quit button connected successfully!");
        }
        else
        {
            Debug.LogError("Quit Button not found! Please assign it in Inspector or check GameObject name.");
        }
    }

    void OnQuitClicked()
    {
        Debug.Log("Quit button clicked!");

        // SỬA: Phát âm thanh và delay quit
        PlayButtonSound();

        // Delay để âm thanh kịp phát
        StartCoroutine(QuitWithDelay(0.3f));
    }

    void PlayButtonSound()
    {
        // SỬA: Thêm debug để kiểm tra
        Debug.Log($"Playing quit button sound. AudioSource: {audioSource != null}, Sound: {buttonClickSound != null}");

        if (buttonClickSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(buttonClickSound, 1.0f);
            Debug.Log("Quit button sound played!");
        }
        else
        {
            if (buttonClickSound == null)
                Debug.LogError("Button Click Sound is NULL! Please assign it in Inspector.");
            if (audioSource == null)
                Debug.LogError("AudioSource is NULL!");
        }
    }

    // SỬA: Thêm coroutine để delay quit
    IEnumerator QuitWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        Debug.Log("Quitting application...");
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    // SỬA: Thêm hàm test
    [ContextMenu("Test Quit Button Sound")]
    void TestQuitButtonSound()
    {
        PlayButtonSound();
    }
}

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayButtonHandler : MonoBehaviour
{
    [Header("UI References")]
    public Button playButton;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip buttonClickSound;

    void Start()
    {
        SetupAudio();
        playButton.onClick.AddListener(OnPlayClicked);
    }

    void SetupAudio()
    {
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // SỬA: Đảm bảo AudioSource được cấu hình đúng
        audioSource.playOnAwake = false;
        audioSource.volume = 1.0f;  // Đảm bảo volume = 1
        audioSource.mute = false;   // Đảm bảo không bị mute

        Debug.Log("AudioSource setup completed");
    }

    void OnPlayClicked()
    {
        Debug.Log("Play button clicked!");

        // SỬA: Phát âm thanh và delay chuyển scene
        PlayButtonSound();

        // Delay chuyển scene để âm thanh kịp phát
        StartCoroutine(LoadSceneWithDelay("PlayScene", 0.2f));
    }

    void PlayButtonSound()
    {
        // SỬA: Thêm debug và kiểm tra kỹ hơn
        Debug.Log($"Attempting to play sound. AudioSource: {audioSource != null}, Sound: {buttonClickSound != null}");

        if (buttonClickSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(buttonClickSound, 1.0f); // Volume = 1.0
            Debug.Log("Button sound played!");
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
        // Chuyển sang game music trước
        if (BackgroundMusicManager.Instance != null)
        {
            BackgroundMusicManager.Instance.PlayGameMusic();
        }

        // Đợi âm thanh phát xong
        yield return new WaitForSeconds(delay);

        Debug.Log($"Loading scene: {sceneName}");
        SceneManager.LoadScene(sceneName);
    }
}

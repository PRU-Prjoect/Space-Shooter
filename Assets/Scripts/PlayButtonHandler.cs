using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
        audioSource.playOnAwake = false;
    }

    void OnPlayClicked()
    {
        PlayButtonSound();

        // Chuyển sang game music
        if (BackgroundMusicManager.Instance != null)
        {
            BackgroundMusicManager.Instance.PlayGameMusic();
        }

        Debug.Log("PlayButton pressed!");
        SceneManager.LoadScene("PlayScene");
    }

    void PlayButtonSound()
    {
        if (buttonClickSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(buttonClickSound);
        }
    }
}

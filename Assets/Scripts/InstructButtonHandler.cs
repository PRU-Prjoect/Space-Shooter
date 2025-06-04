using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InstructButtonHandler : MonoBehaviour
{
    public Button instructButton;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip buttonClickSound;

    void Start()
    {
        if (instructButton != null)
        {
            instructButton.onClick.AddListener(OnInstructClicked);
        }
        else
        {
            Debug.LogError("Instruct Button not assigned!");
        }
    }

    void OnInstructClicked()
    {
        PlayButtonSound();
        Debug.Log("Instruct button clicked! Loading instruction scene...");
        SceneManager.LoadScene("InstructScene");
    }
    void PlayButtonSound()
    {
        if (buttonClickSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(buttonClickSound);
        }
    }
}

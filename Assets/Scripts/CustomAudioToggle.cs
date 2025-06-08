using UnityEngine;
using UnityEngine.UI;

public class CustomAudioToggle : MonoBehaviour
{
    [SerializeField] private Button toggleButton;
    [SerializeField] private Sprite soundOnSprite;
    [SerializeField] private Sprite soundOffSprite;

    private bool isSoundOn = true;

    void Start()
    {
        toggleButton.onClick.AddListener(ToggleSound);
        UpdateButtonSprite();
    }

    void ToggleSound()
    {
        isSoundOn = !isSoundOn;
        AudioListener.volume = isSoundOn ? 1f : 0f;
        UpdateButtonSprite();
    }

    void UpdateButtonSprite()
    {
        toggleButton.GetComponent<Image>().sprite = isSoundOn ? soundOnSprite : soundOffSprite;
    }
}

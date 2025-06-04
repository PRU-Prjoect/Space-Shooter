using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayButtonHandler : MonoBehaviour
{
    public Button playButton; // Gắn nút PLAY ở Inspector

    void Start()
    {
        playButton.onClick.AddListener(OnPlayClicked);
    }

    void OnPlayClicked()
    {
        Debug.Log("PlayButton press!");
        SceneManager.LoadScene("GameScene"); // Đổi "GameScene" thành tên scene chơi của bạn
    }
}

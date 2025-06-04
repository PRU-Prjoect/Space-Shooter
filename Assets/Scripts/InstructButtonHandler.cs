using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InstructButtonHandler : MonoBehaviour
{
    public Button instructButton;
    public GameObject instructPanel; // Panel hiển thị hướng dẫn

    void Awake()
    {
        //instructButton.onClick.AddListener(OnInstructClicked);
    }

    void OnInstructClicked()
    {
        Debug.Log("Instruct button clicked!");

        // Hiển thị panel hướng dẫn
        if (instructPanel != null)
        {
            instructPanel.SetActive(true);
        }

        // Hoặc chuyển đến scene hướng dẫn riêng
        // SceneManager.LoadScene("InstructScene");
    }
}

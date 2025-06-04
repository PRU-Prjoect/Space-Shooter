using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InstructButtonHandler : MonoBehaviour
{
    public Button instructButton;

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
        Debug.Log("Instruct button clicked! Loading instruction scene...");
        SceneManager.LoadScene("InstructScene");
    }
}

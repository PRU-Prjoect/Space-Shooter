using UnityEngine;
using UnityEngine.UI;

public class InstructButtonHandler : MonoBehaviour
{
    public Button instructButton;            // Gắn nút Instruct
    public GameObject instructionPanel;      // Gắn panel hướng dẫn

    void Start()
    {
        instructButton.onClick.AddListener(ToggleInstruction);
        instructionPanel.SetActive(false); // Ẩn panel ban đầu
    }

    void ToggleInstruction()
    {
        bool isActive = instructionPanel.activeSelf;
        instructionPanel.SetActive(!isActive);
        Debug.Log("Instruct clicked → " + (!isActive ? "Hiện" : "Ẩn") + " Instruct");
    }
}

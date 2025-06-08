using UnityEngine;
using UnityEngine.UI;

public class PlaneSelectionPopup : MonoBehaviour
{
    [SerializeField] private GameObject popupPanel;
    [SerializeField] private Button optionButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button[] planeButtons;
    [SerializeField] private GameObject[] planeModels;

    void Start()
    {
        optionButton.onClick.AddListener(OpenPopup);
        closeButton.onClick.AddListener(ClosePopup);

        // Gắn sự kiện cho từng nút máy bay
        for (int i = 0; i < planeButtons.Length; i++)
        {
            int index = i;
            planeButtons[i].onClick.AddListener(() => SelectPlane(index));
        }

        popupPanel.SetActive(false);
    }

    void OpenPopup()
    {
        popupPanel.SetActive(true);
        Time.timeScale = 0f; // Tạm dừng game
    }

    void ClosePopup()
    {
        popupPanel.SetActive(false);
        Time.timeScale = 1f; // Tiếp tục game
    }

    void SelectPlane(int index)
    {
        // Ẩn tất cả máy bay
        foreach (GameObject plane in planeModels)
        {
            plane.SetActive(false);
        }

        // Hiện máy bay được chọn
        planeModels[index].SetActive(true);

        ClosePopup();
        Debug.Log($"Đã chọn máy bay {index}");
    }
}

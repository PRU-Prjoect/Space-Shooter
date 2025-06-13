using UnityEngine;
using UnityEngine.UI; // Bắt buộc phải có để làm việc với Image và Text

public class PlaneSelectionPopup : MonoBehaviour
{
    public GameObject optionsPanel;
    // Kéo các Sprite máy bay của bạn vào đây trong Inspector
    public Sprite[] airplaneSprites;

    // Kéo đối tượng Image dùng để hiển thị máy bay vào đây
    public Image airplaneDisplayImage;

    // (Tùy chọn) Kéo đối tượng Text để hiển thị tên máy bay
    public Text airplaneNameText;

    private int currentIndex = 0;

    void Start()
    {
        // Đảm bảo rằng có ít nhất một sprite được gán
        if (airplaneSprites.Length > 0)
        {
            ShowAirplane(currentIndex);
        }
    }

    // Hàm chính để cập nhật hình ảnh hiển thị
    private void ShowAirplane(int index)
    {
        // Gán sprite từ mảng vào component Image
        airplaneDisplayImage.sprite = airplaneSprites[index];

        // (Tùy chọn) Cập nhật tên. Tên được lấy từ tên của file Sprite.
        if (airplaneNameText != null)
        {
            airplaneNameText.text = airplaneSprites[index].name;
        }
    }

    // Hàm được gọi bởi nút Next
    public void NextAirplane()
    {
        currentIndex++;
        if (currentIndex >= airplaneSprites.Length)
        {
            currentIndex = 0; // Quay về đầu danh sách
        }
        ShowAirplane(currentIndex);
    }

    // Hàm được gọi bởi nút Previous
    public void PreviousAirplane()
    {
        currentIndex--;
        if (currentIndex < 0)
        {
            currentIndex = airplaneSprites.Length - 1; // Đi đến cuối danh sách
        }
        ShowAirplane(currentIndex);
    }

    // Hàm được gọi bởi nút Select
    // Tìm hàm SelectAirplane() và thay thế nội dung của nó bằng đoạn code này
    public void SelectAirplane()
    {
        // 1. Lưu lựa chọn của người chơi
        // PlayerPrefs là một hệ thống lưu trữ dữ liệu đơn giản, hoạt động như một cặp key-value.
        // Đây là một dạng quản lý dữ liệu cơ bản trong Unity[3].
        PlayerPrefs.SetInt("SelectedAirplaneIndex", currentIndex);
        PlayerPrefs.Save(); // Luôn gọi Save() để đảm bảo dữ liệu được ghi vào ổ đĩa

        Debug.Log("Đã chọn máy bay có index: " + currentIndex + " và đã lưu lại.");        
        optionsPanel.SetActive(false); // Ra lệnh cho panel tự ẩn đi
    }

}

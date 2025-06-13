using UnityEngine;
using UnityEngine.UI; // Bắt buộc phải có để làm việc với Image và Text

public class PlaneSelectionPopup : MonoBehaviour
{
    // === CÁC BIẾN PUBLIC ĐỂ KÉO THẢ TRONG EDITOR (GIỮ NGUYÊN) ===
    public GameObject optionsPanel;
    public Sprite[] airplaneSprites; // Giữ nguyên cách kéo thả thủ công
    public Image airplaneDisplayImage;
    public Text airplaneNameText;

    [Header("Audio")]
    public AudioClip buttonClickSound;

    // === CÁC BIẾN PRIVATE (GIỮ NGUYÊN) ===
    private AudioSource audioSource;
    private int currentIndex = 0;

    // THÊM MỚI: Sử dụng Awake() để khởi tạo các component cần thiết
    void Awake()
    {
        // Lấy component AudioSource từ chính GameObject này (AirplaneManager)
        audioSource = GetComponent<AudioSource>();
        // Nếu không tìm thấy, tự động thêm vào để tránh lỗi NullReferenceException
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Start()
    {
        if (airplaneSprites.Length > 0)
        {
            ShowAirplane(currentIndex);
        }
    }

    // Hàm này không thay đổi
    private void ShowAirplane(int index)
    {
        airplaneDisplayImage.sprite = airplaneSprites[index];
        if (airplaneNameText != null)
        {
            airplaneNameText.text = airplaneSprites[index].name;
        }
    }

    // Hàm này không thay đổi
    private void PlaySound()
    {
        if (buttonClickSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(buttonClickSound);
        }
    }

    public void NextAirplane()
    {
        PlaySound(); // THÊM MỚI: Gọi hàm phát âm thanh
        currentIndex++;
        if (currentIndex >= airplaneSprites.Length)
        {
            currentIndex = 0;
        }
        ShowAirplane(currentIndex);
    }

    public void PreviousAirplane()
    {
        PlaySound(); // THÊM MỚI: Gọi hàm phát âm thanh
        currentIndex--;
        if (currentIndex < 0)
        {
            currentIndex = airplaneSprites.Length - 1;
        }
        ShowAirplane(currentIndex);
    }

    // Hàm SelectAirplane của bạn đã đúng, chỉ cần thêm gọi hàm âm thanh
    public void SelectAirplane()
    {
        PlaySound(); // THÊM MỚI: Gọi hàm phát âm thanh

        // Lưu lựa chọn của người chơi
        PlayerPrefs.SetInt("SelectedAirplaneIndex", currentIndex);
        PlayerPrefs.Save();

        Debug.Log("Đã chọn máy bay có index: " + currentIndex + " và đã lưu lại.");

        // Kiểm tra để chắc chắn rằng optionsPanel đã được gán trước khi sử dụng
        if (optionsPanel != null)
        {
            optionsPanel.SetActive(false); // Đóng panel
        }
    }

    // THÊM MỚI: Tạo một hàm riêng cho nút Close để nó cũng có thể phát âm thanh
    public void ClosePanel()
    {
        PlaySound();
        if (optionsPanel != null)
        {
            optionsPanel.SetActive(false);
        }
    }
}

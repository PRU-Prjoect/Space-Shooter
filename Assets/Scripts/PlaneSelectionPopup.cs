using UnityEngine;
using UnityEngine.UI;

public class PlaneSelectionPopup : MonoBehaviour
{
    [Header("UI References")]
    public Button previousButton;
    public Button nextButton;
    public Button closeButton;
    public Transform planeDisplayArea;

    [Header("Plane Settings")]
    public GameObject[] planePrefabs;

    private int selectedPlaneIndex = 0;
    private GameObject currentPlaneDisplay;

    void Start()
    {
        // Gán sự kiện cho các button
        previousButton.onClick.AddListener(PreviousPlane);
        nextButton.onClick.AddListener(NextPlane);
        closeButton.onClick.AddListener(ClosePanel);

        // Hiển thị máy bay đầu tiên
        ShowPlane(selectedPlaneIndex);
    }

    void ShowPlane(int index)
    {
        // Xóa máy bay hiện tại
        if (currentPlaneDisplay != null)
            Destroy(currentPlaneDisplay);

        // Hiển thị máy bay mới
        currentPlaneDisplay = Instantiate(planePrefabs[index], planeDisplayArea.position, planeDisplayArea.rotation);
        currentPlaneDisplay.transform.SetParent(planeDisplayArea);

        // Lưu lựa chọn
        PlayerPrefs.SetInt("SelectedPlane", selectedPlaneIndex);
    }

    public void NextPlane()
    {
        selectedPlaneIndex = (selectedPlaneIndex + 1) % planePrefabs.Length;
        ShowPlane(selectedPlaneIndex);
    }

    public void PreviousPlane()
    {
        selectedPlaneIndex--;
        if (selectedPlaneIndex < 0)
            selectedPlaneIndex = planePrefabs.Length - 1;
        ShowPlane(selectedPlaneIndex);
    }

    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }
}

using UnityEngine;

[System.Serializable]
public class Background : MonoBehaviour
{
    [Header("Screen Settings")]
    [HideInInspector] public Vector3 MaxPoint;
    [HideInInspector] public Vector3 MinPoint;

    [Header("Scroll Settings")]
    public Material[] backgroundMaterials;
    public float scrollSpeed = 2.0f;
    public Vector2 scrollDirection = Vector2.left; // Đổi về left để scroll như game máy bay

    [Header("Background Info")]
    [HideInInspector] public int currentIndex = 0; // Public để ScoreManager đọc được

    private Material currentMaterial;
    private Vector2 offset;
    private MeshRenderer meshRenderer;

    void Start()
    {
        InitializeScreenBounds();
        ScaleBackground();
        InitializeScrolling();
    }

    void Update()
    {
        // CHỈ SCROLL, KHÔNG TỰ ĐỘNG CHUYỂN NỀN
        HandleScrolling();
    }

    void InitializeScreenBounds()
    {
        if (Camera.main == null)
        {
            Debug.LogError("Main Camera not found!");
            return;
        }

        meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            Bounds bounds = meshRenderer.bounds;
            MaxPoint = bounds.max;
            MinPoint = bounds.min;
        }
        else
        {
            MaxPoint = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight, 0.0f));
            MinPoint = Camera.main.ScreenToWorldPoint(Vector3.zero);
        }
    }

    void ScaleBackground()
    {
        meshRenderer = GetComponent<MeshRenderer>();

        float cameraHeight = 2f * Camera.main.orthographicSize;
        float cameraWidth = cameraHeight * Camera.main.aspect;

        transform.localScale = new Vector3(cameraWidth, cameraHeight, 1f);
    }

    // ĐỔI TÊN VÀ XÓA LOGIC TỰ ĐỘNG CHUYỂN NỀN
    void HandleScrolling()
    {
        if (currentMaterial == null) return;

        // CHỈ SCROLL TEXTURE, KHÔNG CHUYỂN NỀN TỰ ĐỘNG
        offset += scrollDirection * scrollSpeed * Time.deltaTime;
        currentMaterial.mainTextureOffset = offset;

        // XÓA ĐOẠN CODE NÀY - NGUYÊN NHÂN GÂY TỰ ĐỘNG ĐỔI NỀN
        /*
        float progress = GetScrollProgress();
        if (progress >= 1.0f)
        {
            NextBackground(); // XÓA DÒNG NÀY
        }
        */
    }

    // GIỮ LẠI HÀM NÀY NHƯNG KHÔNG DÙNG TRONG UPDATE
    float GetScrollProgress()
    {
        if (scrollDirection.x != 0) return Mathf.Abs(offset.x);
        if (scrollDirection.y != 0) return Mathf.Abs(offset.y);
        return 0f;
    }

    void InitializeScrolling()
    {
        if (backgroundMaterials.Length > 0)
        {
            meshRenderer.material = backgroundMaterials[0];
            currentMaterial = meshRenderer.material;
            currentIndex = 0;
        }

        offset = Vector2.zero;
        Debug.Log($"Background initialized with material: {backgroundMaterials[0].name}");
    }

    // HÀM NÀY CHỈ ĐƯỢC GỌI TỪ SCOREMANAGER
    public void NextBackground()
    {
        Debug.Log("NextBackground() called from ScoreManager"); // Debug để kiểm tra

        if (backgroundMaterials.Length <= 1) return;

        currentIndex = (currentIndex + 1) % backgroundMaterials.Length;

        meshRenderer.material = backgroundMaterials[currentIndex];
        currentMaterial = meshRenderer.material;

        offset = Vector2.zero;
        currentMaterial.mainTextureOffset = offset;

        Debug.Log($"Background changed to: {backgroundMaterials[currentIndex].name} (Index: {currentIndex})");
    }

    // HÀM ĐỂ RESET VỀ BACKGROUND ĐẦU TIÊN
    public void ResetToFirstBackground()
    {
        if (backgroundMaterials.Length > 0)
        {
            currentIndex = 0;
            meshRenderer.material = backgroundMaterials[0];
            currentMaterial = meshRenderer.material;
            offset = Vector2.zero;
            currentMaterial.mainTextureOffset = offset;
        }
    }
}

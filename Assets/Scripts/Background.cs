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
    public Vector2 scrollDirection = Vector2.left;

    [Header("Background Info")]
    [HideInInspector] public int currentIndex = 0;

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
        UpdateBounds();
    }

    void UpdateBounds()
    {
        if (meshRenderer != null)
        {
            // Cập nhật bounds từ mesh renderer
            Bounds bounds = meshRenderer.bounds;
            MaxPoint = bounds.max;
            MinPoint = bounds.min;
        }
        else
        {
            // Fallback: Tính từ camera
            MaxPoint = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight, 0.0f));
            MinPoint = Camera.main.ScreenToWorldPoint(Vector3.zero);
        }

        Debug.Log($"Background bounds updated: Min={MinPoint}, Max={MaxPoint}");
    }

    void ScaleBackground()
    {
        meshRenderer = GetComponent<MeshRenderer>();

        float cameraHeight = 2f * Camera.main.orthographicSize;
        float cameraWidth = cameraHeight * Camera.main.aspect;

        transform.localScale = new Vector3(cameraWidth, cameraHeight, 1f);

        // Cập nhật bounds sau khi scale
        UpdateBounds();
    }

    void HandleScrolling()
    {
        if (currentMaterial == null) return;

        // Chỉ scroll texture
        offset += scrollDirection * scrollSpeed * Time.deltaTime;
        currentMaterial.mainTextureOffset = offset;
    }

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

    // HÀM ĐƯỢC GỌI TỪ SCOREMANAGER
    public void NextBackground()
    {
        Debug.Log("NextBackground() called from ScoreManager");

        if (backgroundMaterials.Length <= 1) return;

        currentIndex = (currentIndex + 1) % backgroundMaterials.Length;

        meshRenderer.material = backgroundMaterials[currentIndex];
        currentMaterial = meshRenderer.material;

        offset = Vector2.zero;
        currentMaterial.mainTextureOffset = offset;

        // CẬP NHẬT BOUNDS KHI CHUYỂN BACKGROUND
        UpdateBounds();

        Debug.Log($"Background changed to: {backgroundMaterials[currentIndex].name} (Index: {currentIndex})");
    }

    public void ResetToFirstBackground()
    {
        if (backgroundMaterials.Length > 0)
        {
            currentIndex = 0;
            meshRenderer.material = backgroundMaterials[0];
            currentMaterial = meshRenderer.material;
            offset = Vector2.zero;
            currentMaterial.mainTextureOffset = offset;

            // Cập nhật bounds khi reset
            UpdateBounds();
        }
    }

    // HÀM TIỆN ÍCH ĐỂ CÁC SCRIPT KHÁC LẤY BOUNDS
    public Vector3 GetMaxPoint()
    {
        return MaxPoint;
    }

    public Vector3 GetMinPoint()
    {
        return MinPoint;
    }

    public bool IsInsideBounds(Vector3 position, float buffer = 0f)
    {
        return position.x >= MinPoint.x - buffer &&
               position.x <= MaxPoint.x + buffer &&
               position.y >= MinPoint.y - buffer &&
               position.y <= MaxPoint.y + buffer;
    }

    // DEBUG: Vẽ bounds trong Scene view
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Vector3 center = (MaxPoint + MinPoint) / 2f;
        Vector3 size = MaxPoint - MinPoint;
        Gizmos.DrawWireCube(center, size);
    }
}

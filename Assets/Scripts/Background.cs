using UnityEngine;

[System.Serializable]
public class Background : MonoBehaviour
{
    [Header("Screen Settings")]
    [HideInInspector] public Vector3 MaxPoint;
    [HideInInspector] public Vector3 MinPoint;

    [Header("Scroll Settings")]
    public Sprite[] backgroundSprites; // Thay Material bằng Sprite
    public float scrollSpeed = 2.0f;
    public Vector2 scrollDirection = Vector2.left;

    private Vector3 startPosition;
    private float backgroundWidth;
    private int currentIndex = 0;
    private SpriteRenderer spriteRenderer;
    private GameObject[] backgroundObjects;

    void Start()
    {
        InitializeScreenBounds();
        SetupInfiniteScrolling();
    }

    void Update()
    {
        HandleInfiniteScrolling();
    }

    void InitializeScreenBounds()
    {
        if (Camera.main == null)
        {
            Debug.LogError("Main Camera not found!");
            return;
        }

        MaxPoint = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight, 0.0f));
        MinPoint = Camera.main.ScreenToWorldPoint(Vector3.zero);
    }

    void SetupInfiniteScrolling()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Scale background để fit màn hình
        float cameraHeight = 2f * Camera.main.orthographicSize;
        float cameraWidth = cameraHeight * Camera.main.aspect;

        if (backgroundSprites.Length > 0)
        {
            spriteRenderer.sprite = backgroundSprites[0];
            backgroundWidth = spriteRenderer.bounds.size.x;

            // Tạo 3 background objects để scroll vô tận
            backgroundObjects = new GameObject[3];
            for (int i = 0; i < 3; i++)
            {
                if (i == 0)
                {
                    backgroundObjects[i] = gameObject; // Object hiện tại
                }
                else
                {
                    backgroundObjects[i] = Instantiate(gameObject);
                    backgroundObjects[i].transform.position = new Vector3(i * backgroundWidth, 0, 0);
                }
            }
        }

        startPosition = transform.position;
    }

    void HandleInfiniteScrolling()
    {
        // Di chuyển tất cả background objects
        for (int i = 0; i < backgroundObjects.Length; i++)
        {
            backgroundObjects[i].transform.position += (Vector3)scrollDirection * scrollSpeed * Time.deltaTime;

            // Reset position khi object ra khỏi màn hình
            if (backgroundObjects[i].transform.position.x <= -backgroundWidth * 2)
            {
                float rightmostX = GetRightmostX();
                backgroundObjects[i].transform.position = new Vector3(rightmostX + backgroundWidth, 0, 0);

                // Đổi sprite khi reset position
                ChangeBackgroundSprite(backgroundObjects[i]);
            }
        }
    }

    float GetRightmostX()
    {
        float rightmostX = backgroundObjects[0].transform.position.x;
        for (int i = 1; i < backgroundObjects.Length; i++)
        {
            if (backgroundObjects[i].transform.position.x > rightmostX)
                rightmostX = backgroundObjects[i].transform.position.x;
        }
        return rightmostX;
    }

    void ChangeBackgroundSprite(GameObject bgObject)
    {
        if (backgroundSprites.Length <= 1) return;

        currentIndex = (currentIndex + 1) % backgroundSprites.Length;
        bgObject.GetComponent<SpriteRenderer>().sprite = backgroundSprites[currentIndex];
    }
}

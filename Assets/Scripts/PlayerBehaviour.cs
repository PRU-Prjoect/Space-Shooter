using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public class PlayerBehaviour : MonoBehaviour
{
    public GameObject bulletPrefab;
    float speed = 5.0f;
    private Keyboard keyboard;

    [Header("Boundary Settings")]
    public float minX = -8f;
    public float maxX = 8f;
    public float minY = -4f;
    public float maxY = 4f;
    [Header("Health System")]
    private HealthManager healthManager;

    // Hoặc tự động lấy từ Background component
    private Background background;

    [Header("Shooting Settings")]
    public float fireRate = 0.2f; // Số giây giữa các lần bắn
    private float nextFireTime = 0f;

    public Vector3 sizeOfSprite
    {
        get
        {
            return new(this.GetComponent<SpriteRenderer>().sprite.rect.width, this.GetComponent<SpriteRenderer>().sprite.rect.height, 0.0f);
        }
    }

    void Start()
    {
        keyboard = Keyboard.current;

        // Tự động tìm prefab nếu chưa gán
        if (bulletPrefab == null)
        {
            bulletPrefab = Resources.Load<GameObject>("BulletPrefab");
        }

        // Tự động lấy boundary từ Background (nếu có)
        background = Object.FindAnyObjectByType<Background>();
        if (background != null)
        {
            minX = background.MinPoint.x;
            maxX = background.MaxPoint.x;
            minY = background.MinPoint.y;
            maxY = background.MaxPoint.y;
        }
        // Lấy HealthManager component
        healthManager = GetComponent<HealthManager>();
        if (healthManager == null)
        {
            Debug.LogError("HealthManager component not found on Player!");
        }
    }

    void Update()
    {
        if (keyboard == null) return;

        // Xử lý bắn đạn tự động
        if (Time.time >= nextFireTime)
        {
            Fire();
            nextFireTime = Time.time + fireRate;
        }
        // Lưu vị trí hiện tại
        Vector3 currentPosition = transform.position;

        if (keyboard.wKey.isPressed || keyboard.upArrowKey.isPressed)
        {
            currentPosition += new Vector3(0.0f, speed * Time.deltaTime, 0.0f);
        }

        if (keyboard.sKey.isPressed || keyboard.downArrowKey.isPressed)
        {
            currentPosition += Vector3.down * speed * Time.deltaTime;
        }

        if (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed)
        {
            currentPosition += Vector3.left * speed * Time.deltaTime;
        }

        if (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed)
        {
            currentPosition += Vector3.right * speed * Time.deltaTime;
        }

        // Giới hạn vị trí trong boundary
        currentPosition.x = Mathf.Clamp(currentPosition.x, minX, maxX);
        currentPosition.y = Mathf.Clamp(currentPosition.y, minY, maxY);

        // Áp dụng vị trí đã được giới hạn
        transform.position = currentPosition;
    }

    void Fire()
    {
        if (bulletPrefab == null)
        {
            Debug.LogError("Bullet Prefab chưa được gán trong Inspector!");
            return;
        }

        // Tính toán vị trí spawn đạn
        Vector3 spawnPosition = transform.position + Vector3.up * 1.0f;

        // Giới hạn góc bắn theo boundary
        spawnPosition.x = Mathf.Clamp(spawnPosition.x, minX, maxX);
        spawnPosition.y = Mathf.Clamp(spawnPosition.y, minY, maxY);

        GameObject bulletClone = Instantiate(bulletPrefab, spawnPosition, Quaternion.identity);
    }

    public void TakeDamageFromObstacle(int damage)
    {
        if (healthManager != null)
        {
            healthManager.TakeDamage(damage);
        }
        
    }
}

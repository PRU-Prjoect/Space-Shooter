using UnityEngine;

[System.Serializable]
public class BulletBehaviour : MonoBehaviour
{
    [Header("Bullet Settings")]
    public float speed = 8.0f;
    public int bulletDamage = 1;
    public GameObject explosionEffect;

    private Vector3 direction;
    private Background background;
    private float limitY, limitX, minY, minX; // Giới hạn từ background

    public Vector3 sizeOfSprite
    {
        get
        {
            return GetComponent<SpriteRenderer>().bounds.size;
        }
    }

    void Start()
    {
        // Lấy boundary từ Background
        background = Object.FindFirstObjectByType<Background>();
        if (background != null)
        {
            // SỬA: Giảm buffer để giới hạn chặt hơn trong background
            limitY = background.MaxPoint.y - 0.5f; // Giảm từ +2f xuống -0.5f
            limitX = background.MaxPoint.x - 0.5f; // Giảm từ +2f xuống -0.5f
            minY = background.MinPoint.y + 0.5f;   // Tăng từ -2f lên +0.5f
            minX = background.MinPoint.x + 0.5f;   // Tăng từ -2f lên +0.5f
        }
        else
        {
            // Fallback nếu không tìm thấy background
            limitY = 10f;  // Giảm từ 15f
            limitX = 8f;   // Giảm từ 15f
            minY = -4f;    // Tăng từ -15f
            minX = -8f;    // Tăng từ -15f
        }

        // Tính hướng bay dựa trên rotation
        float angle = transform.eulerAngles.z * Mathf.Deg2Rad;
        direction = new Vector3(-Mathf.Sin(angle), Mathf.Cos(angle), 0).normalized;

        if (direction.magnitude < 0.1f)
        {
            direction = Vector3.up;
        }

        Debug.Log($"Bullet boundary (tightened): X({minX}, {limitX}), Y({minY}, {limitY})");
    }

    void Update()
    {
        // Bay theo hướng đã tính
        transform.position += direction * speed * Time.deltaTime;

        // SỬA: Kiểm tra giới hạn background chặt chẽ hơn
        Vector3 pos = transform.position;
        if (pos.y >= limitY || pos.x >= limitX ||
            pos.x <= minX || pos.y <= minY)
        {
            Debug.Log($"Bullet destroyed at background boundary - Position: {pos}");
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // SỬA: Thêm kiểm tra Player để tránh va chạm
        if (other.CompareTag("Player"))
        {
            Debug.Log("Bullet ignored collision with Player");
            return; // Bỏ qua va chạm với Player
        }

        if (other.CompareTag("Obstacles"))
        {
            Debug.Log("Bullet hit obstacle!");

            if (explosionEffect != null)
            {
                GameObject explosion = Instantiate(explosionEffect, transform.position, Quaternion.identity);
                Destroy(explosion, 2f);
            }

            Destroy(gameObject);
        }
    }

    // THÊM: Hàm để vẽ boundary trong Scene view (debug)
    void OnDrawGizmos()
    {
        if (background != null)
        {
            Gizmos.color = Color.red;
            Vector3 center = new Vector3((minX + limitX) / 2, (minY + limitY) / 2, 0);
            Vector3 size = new Vector3(limitX - minX, limitY - minY, 0);
            Gizmos.DrawWireCube(center, size);
        }
    }
}

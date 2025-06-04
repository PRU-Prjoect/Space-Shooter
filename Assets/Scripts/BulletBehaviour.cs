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
            limitY = background.MaxPoint.y + 2f; // Thêm buffer 2 units
            limitX = background.MaxPoint.x + 2f;
            minY = background.MinPoint.y - 2f;
            minX = background.MinPoint.x - 2f;
        }
        else
        {
            // Fallback nếu không tìm thấy background
            limitY = 15f;
            limitX = 15f;
            minY = -15f;
            minX = -15f;
        }

        // Tính hướng bay dựa trên rotation
        float angle = transform.eulerAngles.z * Mathf.Deg2Rad;
        direction = new Vector3(-Mathf.Sin(angle), Mathf.Cos(angle), 0).normalized;

        if (direction.magnitude < 0.1f)
        {
            direction = Vector3.up;
        }

        Debug.Log($"Bullet boundary: X({minX}, {limitX}), Y({minY}, {limitY})");
    }

    void Update()
    {
        // Bay theo hướng đã tính
        transform.position += direction * speed * Time.deltaTime;

        // Kiểm tra giới hạn background
        if (transform.position.y >= limitY ||
            transform.position.x >= limitX ||
            transform.position.x <= minX ||
            transform.position.y <= minY)
        {
            Debug.Log("Bullet destroyed at background boundary");
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
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
}

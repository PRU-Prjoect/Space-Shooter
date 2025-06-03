using UnityEngine;

[System.Serializable]
public class BulletBehaviour : MonoBehaviour
{
    float LimitY;
    float speed = 5.0f;
    public GameObject explosionEffect; // Kéo prefab hiệu ứng nổ vào đây

    [Header("Bullet Settings")]
    public int bulletDamage = 1; // Sát thương của đạn

    [HideInInspector]
    public Vector3 sizeOfSprite
    {
        get
        {
            // Sử dụng bounds.size để lấy kích thước thực tế
            return GetComponent<SpriteRenderer>().bounds.size;
        }
    }

    void Start()
    {
        // Giữ nguyên logic tính LimitY
        Background bg = Object.FindFirstObjectByType<Background>();
        if (bg != null)
        {
            LimitY = bg.MaxPoint.y + sizeOfSprite.y / 2.0f;
        }
        else
        {
            LimitY = 10.0f;
        }
    }

    void Update()
    {
        // Di chuyển lên trên (giữ nguyên)
        transform.position += Vector3.up * speed * Time.deltaTime;

        // Hủy khi ra khỏi màn hình (giữ nguyên)
        if (transform.position.y >= LimitY)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Obstacles"))
        {
            // Tạo hiệu ứng nổ
            if (explosionEffect != null)
            {
                Instantiate(explosionEffect, transform.position, Quaternion.identity);
            }

            Destroy(gameObject);
        }
    }

}


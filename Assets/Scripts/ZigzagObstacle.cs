using UnityEngine;

public class ZigzagObstacle : MonoBehaviour
{
    public float fallSpeed = 3.0f;
    public float zigzagSpeed = 2.0f;
    public float zigzagRange = 3.0f;
    public float destroyY = -10.0f;
    public int damageAmount = 30; // Damage gây ra cho player
    public int maxHealth = 4; // Số lần bị bắn mới bể
    private int currentHealth;

    private float startX;

    void Start()
    {
        startX = transform.position.x;
        currentHealth = maxHealth; // Khởi tạo máu đầy
    }

    void Update()
    {
        // Rơi xuống
        transform.position += Vector3.down * fallSpeed * Time.deltaTime;

        // Di chuyển zigzag
        float zigzagX = startX + Mathf.Sin(Time.time * zigzagSpeed) * zigzagRange;
        transform.position = new Vector3(zigzagX, transform.position.y, transform.position.z);

        if (transform.position.y <= destroyY)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Va chạm với player
        if (other.CompareTag("Player"))
        {
            PlayerBehaviour player = other.GetComponent<PlayerBehaviour>();
            if (player != null)
            {
                player.TakeDamageFromObstacle(damageAmount);
            }
            Debug.Log("Player hit zigzag obstacle! Damage: " + damageAmount);
            // Obstacle vẫn tồn tại sau khi va chạm với player
        }
        // Va chạm với đạn
        else if (other.CompareTag("Bullet"))
        {
            TakeDamageFromBullet(1); // Mỗi viên đạn trừ 1 máu
            Destroy(other.gameObject); // Hủy đạn
        }
    }

    // Hàm trừ máu khi bị bắn
    public void TakeDamageFromBullet(int damage)
    {
        currentHealth -= damage;
        Debug.Log($"Zigzag Obstacle hit! Health: {currentHealth}/{maxHealth}");

        // Hiệu ứng khi bị bắn
        FlashEffect();

        if (currentHealth <= 0)
        {
            Debug.Log("Zigzag Obstacle destroyed!");
            Destroy(gameObject);
        }
    }

    // Hiệu ứng nhấp nháy khi bị bắn
    void FlashEffect()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            StartCoroutine(Flash(sr));
        }
    }

    System.Collections.IEnumerator Flash(SpriteRenderer sr)
    {
        Color originalColor = sr.color;
        sr.color = Color.yellow; // Màu vàng để phân biệt với các obstacle khác
        yield return new WaitForSeconds(0.1f);
        sr.color = originalColor;
    }
}

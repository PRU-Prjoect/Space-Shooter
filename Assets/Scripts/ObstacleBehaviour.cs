using UnityEngine;

public class ObstacleBehaviour : MonoBehaviour
{
    public float fallSpeed = 5.0f;
    public float destroyY = -10.0f;
    public int damageAmount = 20; // Damage gây ra cho player
    public int maxHealth = 3; // Số lần bị bắn mới bể
    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth; // Khởi tạo máu đầy
    }

    void Update()
    {
        transform.position += Vector3.down * fallSpeed * Time.deltaTime;

        if (transform.position.y <= destroyY)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerBehaviour player = other.GetComponent<PlayerBehaviour>();
            if (player != null)
            {
                player.TakeDamageFromObstacle(damageAmount);
            }
        }
        else if (other.CompareTag("Bullet"))
        {
            // Cộng điểm khi bắn trúng obstacle
            ScoreManager scoreManager = Object.FindFirstObjectByType<ScoreManager>();
            if (scoreManager != null)
            {
                scoreManager.AddScore(scoreManager.scorePerObstacle);
            }

            TakeDamageFromBullet(1);
            Destroy(other.gameObject);
        }
    }


    // Hàm trừ máu khi bị bắn
    public void TakeDamageFromBullet(int damage)
    {
        currentHealth -= damage;
        Debug.Log($"Obstacle hit! Health: {currentHealth}/{maxHealth}");

        // Hiệu ứng khi bị bắn (tùy chọn)
        FlashEffect();

        if (currentHealth <= 0)
        {
            Debug.Log("Obstacle destroyed!");
            Destroy(gameObject);
        }
    }

    // Hiệu ứng nhấp nháy khi bị bắn (tùy chọn)
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
        sr.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sr.color = Color.white;
    }
}

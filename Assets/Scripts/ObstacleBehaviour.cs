using UnityEngine;

public class ObstacleBehaviour : MonoBehaviour
{
    [Header("Movement Settings")]
    public float fallSpeed = 5.0f;
    public float destroyY = -10.0f;

    [Header("Combat Settings")]
    public int damageAmount = 20; // Damage gây ra cho player
    public int maxHealth = 3; // Số lần bị bắn mới bể
    private int currentHealth;

    [Header("Effects")]
    public GameObject explosionEffect; // Hiệu ứng nổ khi bị phá hủy
    public GameObject hitEffect; // Hiệu ứng khi bị bắn

    [Header("Audio")]
    public AudioClip hitSound; // Âm thanh khi bị bắn
    public AudioClip destroySound; // Âm thanh khi bị phá hủy

    // Cache references để tối ưu performance
    private ScoreManager scoreManager;
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;

    void Start()
    {
        currentHealth = maxHealth;

        // Cache references
        scoreManager = Object.FindFirstObjectByType<ScoreManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Setup AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.playOnAwake = false;
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
            if (scoreManager != null)
            {
                scoreManager.AddScore(scoreManager.scorePerObstacle);
            }

            TakeDamageFromBullet(1);
            Destroy(other.gameObject);
        }
    }

    // THÊM: Hàm set tốc độ từ ObstacleSpawner
    public void SetSpeed(float newSpeed)
    {
        fallSpeed = newSpeed;
    }

    // Hàm trừ máu khi bị bắn - CẢI TIẾN
    public void TakeDamageFromBullet(int damage)
    {
        currentHealth -= damage;
        Debug.Log($"Obstacle hit! Health: {currentHealth}/{maxHealth}");

        // Phát âm thanh khi bị bắn
        PlayHitSound();

        // Hiệu ứng khi bị bắn
        FlashEffect();
        SpawnHitEffect();

        if (currentHealth <= 0)
        {
            Debug.Log("Obstacle destroyed!");
            DestroyObstacle();
        }
    }

    // THÊM: Hàm phá hủy với hiệu ứng
    void DestroyObstacle()
    {
        // Phát âm thanh phá hủy
        PlayDestroySound();

        // Spawn hiệu ứng nổ
        SpawnExplosionEffect();

        // Hủy object
        Destroy(gameObject);
    }

    // THÊM: Phát âm thanh khi bị bắn
    void PlayHitSound()
    {
        if (hitSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(hitSound, 0.7f);
        }
    }

    // THÊM: Phát âm thanh khi bị phá hủy
    void PlayDestroySound()
    {
        if (destroySound != null && audioSource != null)
        {
            audioSource.PlayOneShot(destroySound, 0.8f);
        }
    }

    // THÊM: Spawn hiệu ứng nổ
    void SpawnExplosionEffect()
    {
        if (explosionEffect != null)
        {
            GameObject explosion = Instantiate(explosionEffect, transform.position, Quaternion.identity);
            Destroy(explosion, 2f); // Tự hủy sau 2 giây
        }
    }

    // THÊM: Spawn hiệu ứng khi bị bắn
    void SpawnHitEffect()
    {
        if (hitEffect != null)
        {
            GameObject hit = Instantiate(hitEffect, transform.position, Quaternion.identity);
            Destroy(hit, 1f); // Tự hủy sau 1 giây
        }
    }

    // Hiệu ứng nhấp nháy khi bị bắn - CẢI TIẾN
    void FlashEffect()
    {
        if (spriteRenderer != null)
        {
            StartCoroutine(Flash());
        }
    }

    System.Collections.IEnumerator Flash()
    {
        if (spriteRenderer == null) yield break;

        Color originalColor = spriteRenderer.color;
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);

        // Kiểm tra null trước khi set lại màu
        if (spriteRenderer != null)
        {
            spriteRenderer.color = originalColor;
        }
    }

    // THÊM: Hàm để tối ưu performance (có thể dùng object pooling sau)
    void OnDestroy()
    {
        // Cleanup nếu cần
        StopAllCoroutines();
    }

    // THÊM: Debug info
    void OnDrawGizmos()
    {
        if (Application.isEditor)
        {
            // Vẽ health bar trong Scene view
            Vector3 healthBarPos = transform.position + Vector3.up * 1.5f;
            float healthPercent = (float)currentHealth / maxHealth;

            Gizmos.color = Color.red;
            Gizmos.DrawLine(healthBarPos - Vector3.right * 0.5f,
                           healthBarPos + Vector3.right * 0.5f);

            Gizmos.color = Color.green;
            Gizmos.DrawLine(healthBarPos - Vector3.right * 0.5f,
                           healthBarPos + Vector3.right * (healthPercent - 0.5f));
        }
    }
}

using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("UI References")]
    public Slider healthBar; // Kéo Health Bar UI vào đây
    public Text healthText;  // Text hiển thị số máu (tùy chọn)

    [Header("Damage Settings")]
    public int damagePerHit = 20;
    public bool isInvulnerable = false;
    public float invulnerabilityTime = 1.0f; // Thời gian bất tử sau khi bị hit

    void Start()
    {
        // Khởi tạo máu đầy
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    public void TakeDamage(int damage)
    {
        if (isInvulnerable) return; // Không nhận damage khi bất tử

        currentHealth -= damage;
        currentHealth = Mathf.Max(0, currentHealth); // Không cho âm

        Debug.Log("Player took " + damage + " damage! Current health: " + currentHealth);

        UpdateHealthUI();

        // Kích hoạt bất tử tạm thời
        if (invulnerabilityTime > 0)
        {
            StartCoroutine(InvulnerabilityCoroutine());
        }

        // Kiểm tra chết
        if (currentHealth <= 0)
        {
            PlayerDied();
        }
    }

    void UpdateHealthUI()
    {
        if (healthBar != null)
        {
            healthBar.value = (float)currentHealth / maxHealth;
        }

        if (healthText != null)
        {
            healthText.text = currentHealth + "/" + maxHealth;
        }
    }

    System.Collections.IEnumerator InvulnerabilityCoroutine()
    {
        isInvulnerable = true;

        // Hiệu ứng nhấp nháy (tùy chọn)
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            for (float i = 0; i < invulnerabilityTime; i += 0.1f)
            {
                spriteRenderer.color = new Color(1, 1, 1, 0.5f); // Trong suốt
                yield return new WaitForSeconds(0.05f);
                spriteRenderer.color = new Color(1, 1, 1, 1f);   // Đậm
                yield return new WaitForSeconds(0.05f);
            }
        }
        else
        {
            yield return new WaitForSeconds(invulnerabilityTime);
        }

        isInvulnerable = false;
    }

    void PlayerDied()
    {
        Debug.Log("Player died!");

        // Xử lý khi player chết
        // Ví dụ: restart game, show game over screen, etc.

        // Tạm thời restart scene
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
        );
    }

    // Method để hồi máu (tùy chọn)
    public void Heal(int healAmount)
    {
        currentHealth += healAmount;
        currentHealth = Mathf.Min(maxHealth, currentHealth); // Không vượt quá max
        UpdateHealthUI();
        Debug.Log("Player healed " + healAmount + " HP! Current health: " + currentHealth);
    }
}

using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

[System.Serializable]
public class PlayerBehaviour : MonoBehaviour
{
    [Header("Movement Settings")]
    public GameObject bulletPrefab;
    public float speed = 5.0f;
    private Keyboard keyboard;

    [Header("Boundary Settings")]
    public float minX = -8f;
    public float maxX = 8f;
    public float minY = -4f;
    public float maxY = 4f;

    [Header("Health System")]
    public int health = 100;
    public int maxHealth = 100;
    private HealthManager healthManager;

    // Tự động lấy từ Background component
    private Background background;

    [Header("Shooting Settings")]
    public float fireRate = 0.3f; // TĂNG TỪ 0.2f ĐỂ BẮN CHẬM HƠN
    public Transform firePoint; // THÊM FIRE POINT
    private float nextFireTime = 0f;
    private bool canFire = true;

    [Header("Shield System")]
    public bool hasShield = false;
    public GameObject shieldVisual;

    [Header("Ammo System")]
    public int currentAmmo = 100; // TĂNG TỪ 30 LÊN 100
    public int maxAmmo = 100; // TĂNG TỪ 50 LÊN 100
    public bool infiniteAmmo = false;

    [Header("Triple Shot System")]
    public bool hasTripleShot = false;
    public float tripleShotDuration = 10f;

    [Header("Debug Settings")]
    public bool enableDebugLogs = true;

    public Vector3 sizeOfSprite
    {
        get
        {
            return new Vector3(this.GetComponent<SpriteRenderer>().sprite.rect.width,
                             this.GetComponent<SpriteRenderer>().sprite.rect.height, 0.0f);
        }
    }

    void Start()
    {
        keyboard = Keyboard.current;

        // Tự động tìm prefab nếu chưa gán
        if (bulletPrefab == null)
        {
            bulletPrefab = Resources.Load<GameObject>("BulletPrefab");
            if (bulletPrefab == null)
            {
                Debug.LogError("Bullet Prefab not found! Please assign it in Inspector or place in Resources folder.");
            }
        }

        // Tự động tạo fire point nếu chưa có
        if (firePoint == null)
        {
            GameObject firePointObj = new GameObject("FirePoint");
            firePointObj.transform.SetParent(transform);
            firePointObj.transform.localPosition = Vector3.up * 0.8f; // TĂNG KHOẢNG CÁCH
            firePoint = firePointObj.transform;
            if (enableDebugLogs)
                Debug.Log("Auto-created FirePoint");
        }

        // Tự động lấy boundary từ Background
        UpdateBoundaryFromBackground();

        // Lấy HealthManager component
        healthManager = GetComponent<HealthManager>();

        // Khởi tạo health
        health = maxHealth;

        if (enableDebugLogs)
        {
            Debug.Log($"Player initialized - Ammo: {currentAmmo}/{maxAmmo}, Health: {health}/{maxHealth}");
        }
    }

    void Update()
    {
        if (keyboard == null) return;

        // DEBUG INFO (nhấn I để xem thông tin)
        if (keyboard.iKey.wasPressedThisFrame)
        {
            DebugPlayerInfo();
        }

        // Xử lý bắn đạn
        HandleShooting();

        // Xử lý di chuyển
        HandleMovement();
    }

    void HandleShooting()
    {
        // KIỂM TRA ĐIỀU KIỆN BẮN
        if (Time.time >= nextFireTime && canFire)
        {
            Fire();
            nextFireTime = Time.time + fireRate;
        }
    }

    void UpdateBoundaryFromBackground()
    {
        background = Object.FindAnyObjectByType<Background>();
        if (background != null)
        {
            minX = background.MinPoint.x;
            maxX = background.MaxPoint.x;
            minY = background.MinPoint.y;
            maxY = background.MaxPoint.y;

            if (enableDebugLogs)
            {
                Debug.Log($"Boundary updated: X({minX}, {maxX}), Y({minY}, {maxY})");
            }
        }
    }

    void HandleMovement()
    {
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

        transform.position = currentPosition;
    }

    void Fire()
    {
        if (enableDebugLogs)
        {
            Debug.Log($"Fire attempt - Ammo: {currentAmmo}/{maxAmmo}, Infinite: {infiniteAmmo}, Triple: {hasTripleShot}");
        }

        // KIỂM TRA BULLET PREFAB
        if (bulletPrefab == null)
        {
            Debug.LogError("Bullet Prefab is null! Cannot fire!");
            return;
        }

        // KIỂM TRA AMMO (CHỈ KHI KHÔNG INFINITE)
        if (!infiniteAmmo && currentAmmo <= 0)
        {
            if (enableDebugLogs)
            {
                Debug.LogWarning("No ammo left! Collect ammo powerup.");
            }
            return;
        }

        // BẮN ĐẠN
        if (hasTripleShot)
        {
            FireTripleBullets();
        }
        else
        {
            FireSingleBullet();
        }

        // TRỪ AMMO (CHỈ KHI KHÔNG INFINITE)
        if (!infiniteAmmo)
        {
            currentAmmo--;
            if (enableDebugLogs)
            {
                Debug.Log($"Ammo remaining: {currentAmmo}");
            }
            UpdateAmmoUI();
        }
    }

    void FireSingleBullet()
    {
        // SỬ DỤNG FIRE POINT ĐỂ SPAWN CHÍNH XÁC
        Vector3 spawnPosition = firePoint != null ? firePoint.position : transform.position + Vector3.up * 1.0f;

        GameObject bullet = Instantiate(bulletPrefab, spawnPosition, Quaternion.identity);

        if (bullet != null)
        {
            // ĐẢM BẢO BULLET KHÔNG VA CHẠM VỚI PLAYER
            Collider2D bulletCollider = bullet.GetComponent<Collider2D>();
            Collider2D playerCollider = GetComponent<Collider2D>();

            if (bulletCollider != null && playerCollider != null)
            {
                Physics2D.IgnoreCollision(bulletCollider, playerCollider);
            }

            if (enableDebugLogs)
            {
                Debug.Log($"Single bullet fired at {spawnPosition}");
            }
        }
        else
        {
            Debug.LogError("Failed to instantiate bullet!");
        }
    }

    void FireTripleBullets()
    {
        Vector3 basePosition = firePoint != null ? firePoint.position : transform.position + Vector3.up * 1.0f;

        // Đạn giữa (thẳng)
        GameObject centerBullet = Instantiate(bulletPrefab, basePosition, Quaternion.identity);

        // Đạn trái (nghiêng 15 độ)
        Vector3 leftPosition = basePosition + Vector3.left * 0.3f;
        GameObject leftBullet = Instantiate(bulletPrefab, leftPosition, Quaternion.Euler(0, 0, 15f));

        // Đạn phải (nghiêng -15 độ)
        Vector3 rightPosition = basePosition + Vector3.right * 0.3f;
        GameObject rightBullet = Instantiate(bulletPrefab, rightPosition, Quaternion.Euler(0, 0, -15f));

        // IGNORE COLLISION VỚI PLAYER CHO TẤT CẢ ĐẠN
        GameObject[] bullets = { centerBullet, leftBullet, rightBullet };
        Collider2D playerCollider = GetComponent<Collider2D>();

        foreach (GameObject bullet in bullets)
        {
            if (bullet != null && playerCollider != null)
            {
                Collider2D bulletCollider = bullet.GetComponent<Collider2D>();
                if (bulletCollider != null)
                {
                    Physics2D.IgnoreCollision(bulletCollider, playerCollider);
                }
            }
        }

        if (enableDebugLogs)
        {
            Debug.Log($"Triple bullets fired at {basePosition}");
        }
    }

    // HÀM NHẬN DAMAGE
    public void TakeDamageFromObstacle(int damage)
    {
        if (hasShield)
        {
            Debug.Log("Damage blocked by shield!");
            return;
        }

        if (healthManager != null)
        {
            healthManager.TakeDamage(damage);

            // Kiểm tra nếu hết máu
            if (healthManager.currentHealth <= 0)
            {
                GameOver();
            }
        }
        else
        {
            health -= damage;
            if (health <= 0)
            {
                health = 0;
                GameOver();
            }
            UpdateHealthUI();
        }
    }

    // SHIELD FUNCTIONS
    public void ActivateShield(float duration)
    {
        if (!hasShield)
        {
            hasShield = true;
            if (shieldVisual != null)
                shieldVisual.SetActive(true);

            StartCoroutine(ShieldCoroutine(duration));
            Debug.Log("Shield activated!");
        }
    }

    System.Collections.IEnumerator ShieldCoroutine(float duration)
    {
        yield return new WaitForSeconds(duration);

        hasShield = false;
        if (shieldVisual != null)
            shieldVisual.SetActive(false);

        Debug.Log("Shield deactivated!");
    }

    // TRIPLE SHOT FUNCTIONS
    public void ActivateTripleShot(float duration)
    {
        if (!hasTripleShot)
        {
            hasTripleShot = true;
            StartCoroutine(TripleShotCoroutine(duration));
            Debug.Log("Triple Shot activated!");
        }
    }

    System.Collections.IEnumerator TripleShotCoroutine(float duration)
    {
        yield return new WaitForSeconds(duration);
        hasTripleShot = false;
        Debug.Log("Triple Shot deactivated!");
    }

    // AMMO FUNCTIONS
    public void AddAmmo(int amount)
    {
        currentAmmo = Mathf.Min(currentAmmo + amount, maxAmmo);
        Debug.Log($"Ammo added! Current: {currentAmmo}/{maxAmmo}");
        UpdateAmmoUI();
    }

    public void TripleAmmo()
    {
        int oldAmmo = currentAmmo;
        currentAmmo = Mathf.Min(currentAmmo * 3, maxAmmo);
        Debug.Log($"Ammo tripled! {oldAmmo} → {currentAmmo}");
        UpdateAmmoUI();
    }

    public void RefillAmmo()
    {
        currentAmmo = maxAmmo;
        Debug.Log("Ammo refilled to maximum!");
        UpdateAmmoUI();
    }

    public void ToggleInfiniteAmmo()
    {
        infiniteAmmo = !infiniteAmmo;
        Debug.Log($"Infinite ammo: {infiniteAmmo}");
    }

    // HÀM ĐƯỢC GỌI KHI CHUYỂN BACKGROUND
    public void OnBackgroundChanged()
    {
        UpdateBoundaryFromBackground();

        // REFILL AMMO KHI CHUYỂN BACKGROUND
        if (currentAmmo <= maxAmmo / 2) // Nếu ammo dưới 50%
        {
            AddAmmo(maxAmmo / 2); // Thêm 50% ammo
            Debug.Log("Ammo refilled due to background change");
        }
    }

    void GameOver()
    {
        Debug.Log("Player died! Game Over!");

        // Lưu điểm cuối game
        ScoreManager scoreManager = Object.FindFirstObjectByType<ScoreManager>();
        if (scoreManager != null)
        {
            scoreManager.GameOver();
        }

        // Dừng game tạm thời
        Time.timeScale = 0f;

        // Chuyển đến End Scene sau 2 giây
        Invoke(nameof(LoadEndScene), 2f);
    }

    void LoadEndScene()
    {
        Time.timeScale = 1f; // Reset time scale

        // Sử dụng EndGameCode thay vì SceneController
        int finalScore = PlayerPrefs.GetInt("FinalScore", 0);
        EndGameCode.TriggerGameOver(finalScore);
    }

    public void RestoreHealth(int amount)
    {
        health = Mathf.Min(health + amount, maxHealth);
        UpdateHealthUI();
        Debug.Log($"Health restored! Current: {health}/{maxHealth}");
    }

    // UI UPDATE FUNCTIONS
    void UpdateHealthUI()
    {
        if (enableDebugLogs)
        {
            Debug.Log($"Health: {health}/{maxHealth}");
        }
    }

    void UpdateAmmoUI()
    {
        if (enableDebugLogs)
        {
            if (infiniteAmmo)
            {
                Debug.Log("Ammo: ∞ (Infinite)");
            }
            else
            {
                Debug.Log($"Ammo: {currentAmmo}/{maxAmmo}");
            }
        }
    }

    // DEBUG FUNCTION
    void DebugPlayerInfo()
    {
        Debug.Log("=== PLAYER DEBUG INFO ===");
        Debug.Log($"Position: {transform.position}");
        Debug.Log($"Ammo: {currentAmmo}/{maxAmmo} (Infinite: {infiniteAmmo})");
        Debug.Log($"Health: {health}/{maxHealth}");
        Debug.Log($"Shield: {hasShield}");
        Debug.Log($"Triple Shot: {hasTripleShot}");
        Debug.Log($"Boundary: X({minX}, {maxX}), Y({minY}, {maxY})");
        Debug.Log($"Fire Rate: {fireRate}, Can Fire: {canFire}");
        Debug.Log($"Next fire time: {nextFireTime}, Current time: {Time.time}");
        Debug.Log($"Bullet prefab: {(bulletPrefab != null ? bulletPrefab.name : "NULL")}");
        Debug.Log($"Fire Point: {(firePoint != null ? firePoint.position.ToString() : "NULL")}");
        Debug.Log("========================");
    }

    // UTILITY FUNCTIONS
    public void SetCanFire(bool canFireValue)
    {
        canFire = canFireValue;
        Debug.Log($"Can fire set to: {canFire}");
    }

    public void ResetPlayer()
    {
        health = maxHealth;
        currentAmmo = maxAmmo;
        hasShield = false;
        hasTripleShot = false;
        canFire = true;

        if (shieldVisual != null)
            shieldVisual.SetActive(false);

        Debug.Log("Player reset to initial state");
    }
}

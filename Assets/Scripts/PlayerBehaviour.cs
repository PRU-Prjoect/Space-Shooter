using UnityEngine;
using UnityEngine.InputSystem;

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
    public float fireRate = 0.2f;
    private float nextFireTime = 0f;

    [Header("Shield System")]
    public bool hasShield = false;
    public GameObject shieldVisual;

    [Header("Ammo System")]
    public int currentAmmo = 30;
    public int maxAmmo = 50;
    public bool infiniteAmmo = false; // THÊM OPTION INFINITE AMMO

    [Header("Triple Shot System")]
    public bool hasTripleShot = false;
    public float tripleShotDuration = 10f; // Hiệu ứng kéo dài 10 giây

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
        }

        // Tự động lấy boundary từ Background (nếu có)
        UpdateBoundaryFromBackground();

        // Lấy HealthManager component (nếu có)
        healthManager = GetComponent<HealthManager>();

        // Khởi tạo health
        health = maxHealth;

        // DEBUG INITIAL STATE
        Debug.Log($"Player initialized - Ammo: {currentAmmo}/{maxAmmo}, Boundary: X({minX}, {maxX}), Y({minY}, {maxY})");
    }

    void Update()
    {
        if (keyboard == null) return;

        // DEBUG INFO (nhấn I để xem thông tin)
        if (keyboard.iKey.wasPressedThisFrame)
        {
            DebugPlayerInfo();
        }

        // Xử lý bắn đạn tự động
        if (Time.time >= nextFireTime)
        {
            Fire();
            nextFireTime = Time.time + fireRate;
        }

        // Xử lý di chuyển
        HandleMovement();
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
            Debug.Log($"Boundary updated from background: X({minX}, {maxX}), Y({minY}, {maxY})");
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

        // Áp dụng vị trí đã được giới hạn
        transform.position = currentPosition;
    }

    void Fire()
    {
        Debug.Log($"Fire attempt - Ammo: {currentAmmo}, Triple Shot: {hasTripleShot}, Background: {background?.currentIndex}");

        if (bulletPrefab == null)
        {
            Debug.LogError("Bullet Prefab is null!");
            return;
        }

        if (!infiniteAmmo && currentAmmo <= 0)
        {
            Debug.LogWarning("No ammo left! Need ammo powerup.");
            return;
        }

        // Kiểm tra có Triple Shot không
        if (hasTripleShot)
        {
            FireTripleBullets();
        }
        else
        {
            FireSingleBullet();
        }

        // Trừ ammo
        if (!infiniteAmmo)
        {
            currentAmmo--;
            Debug.Log($"Ammo remaining: {currentAmmo}");
        }
    }

    void FireSingleBullet()
    {
        Vector3 spawnPosition = transform.position + Vector3.up * 0.5f;
        GameObject bullet = Instantiate(bulletPrefab, spawnPosition, Quaternion.identity);

        if (bullet != null)
        {
            Debug.Log($"Single bullet spawned at {spawnPosition}");
        }
        else
        {
            Debug.LogError("Failed to spawn single bullet!");
        }
    }

    void FireTripleBullets()
    {
        Vector3 basePosition = transform.position + Vector3.up * 0.5f;

        // Đạn giữa (thẳng)
        GameObject centerBullet = Instantiate(bulletPrefab, basePosition, Quaternion.identity);

        // Đạn trái (nghiêng 15 độ)
        Vector3 leftPosition = basePosition + Vector3.left * 0.3f;
        GameObject leftBullet = Instantiate(bulletPrefab, leftPosition, Quaternion.Euler(0, 0, 15f));

        // Đạn phải (nghiêng -15 độ)
        Vector3 rightPosition = basePosition + Vector3.right * 0.3f;
        GameObject rightBullet = Instantiate(bulletPrefab, rightPosition, Quaternion.Euler(0, 0, -15f));

        if (centerBullet != null && leftBullet != null && rightBullet != null)
        {
            Debug.Log($"Triple bullets spawned at {basePosition}");
        }
        else
        {
            Debug.LogError("Failed to spawn some triple bullets!");
        }
    }


    // THÊM HÀM NÀY để reset ammo khi chuyển background
    public void OnBackgroundChanged()
    {
        Debug.Log("Background changed - checking ammo");
        if (currentAmmo <= 10) // Nếu ammo thấp, tự động refill
        {
            currentAmmo = maxAmmo;
            Debug.Log("Ammo refilled due to background change");
        }
    }


    // Hàm nhận damage
    public void TakeDamageFromObstacle(int damage)
    {
        if (hasShield)
        {
            Debug.Log("Damage blocked by shield!");
            return; // Shield chặn damage
        }

        // Ưu tiên dùng HealthManager nếu có
        if (healthManager != null)
        {
            healthManager.TakeDamage(damage);
        }
        else
        {
            // Fallback: tự xử lý health
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

    // AMMO FUNCTIONS
    public void AddAmmo(int amount)
    {
        currentAmmo = Mathf.Min(currentAmmo + amount, maxAmmo);
        Debug.Log($"Ammo added! Current: {currentAmmo}/{maxAmmo}");
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

    // UI UPDATE FUNCTIONS
    void UpdateHealthUI()
    {
        Debug.Log($"Health: {health}/{maxHealth}");
    }

    void UpdateAmmoUI()
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

    void GameOver()
    {
        Debug.Log("Game Over!");
        // Thêm logic game over ở đây
    }

    // Hàm hồi phục health
    public void RestoreHealth(int amount)
    {
        health = Mathf.Min(health + amount, maxHealth);
        UpdateHealthUI();
        Debug.Log($"Health restored! Current: {health}/{maxHealth}");
    }

    // DEBUG FUNCTION
    void DebugPlayerInfo()
    {
        Debug.Log("=== PLAYER DEBUG INFO ===");
        Debug.Log($"Position: {transform.position}");
        Debug.Log($"Ammo: {currentAmmo}/{maxAmmo} (Infinite: {infiniteAmmo})");
        Debug.Log($"Health: {health}/{maxHealth}");
        Debug.Log($"Shield: {hasShield}");
        Debug.Log($"Boundary: X({minX}, {maxX}), Y({minY}, {maxY})");
        Debug.Log($"Next fire time: {nextFireTime}, Current time: {Time.time}");
        Debug.Log($"Bullet prefab: {(bulletPrefab != null ? bulletPrefab.name : "NULL")}");
        Debug.Log("========================");
    }
    // THÊM HÀM MỚI
    public void TripleAmmo()
    {
        int oldAmmo = currentAmmo;
        currentAmmo = Mathf.Min(currentAmmo * 3, maxAmmo);
        Debug.Log($"Ammo tripled! {oldAmmo} → {currentAmmo}");
        UpdateAmmoUI();
    }

    public void ActivateTripleShot(float duration)
    {
        hasTripleShot = true;
        Debug.Log("Triple Shot activated!");

        // Hủy coroutine cũ nếu có
        StopCoroutine(nameof(TripleShotCoroutine));
        StartCoroutine(TripleShotCoroutine(duration));
    }

    System.Collections.IEnumerator TripleShotCoroutine(float duration)
    {
        yield return new WaitForSeconds(duration);
        hasTripleShot = false;
        Debug.Log("Triple Shot deactivated!");
    }

}

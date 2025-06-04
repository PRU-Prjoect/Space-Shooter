using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    [Header("PowerUp Prefabs")]
    public GameObject shieldPowerUp;
    public GameObject ammoPowerUp;

    [Header("Spawn Settings")]
    public float spawnInterval = 8f;
    public Transform[] spawnPoints;

    [Header("Spawn Control")] // THÊM MỚI
    public int maxPowerUpsOnScreen = 3; // Giới hạn PowerUp trên màn hình
    private int currentPowerUpCount = 0;

    void Start()
    {
        // Đảm bảo PowerUp hiển thị trên cùng
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.sortingOrder = 10; // Cao hơn background
        }
        // THÊM DEBUG
        Debug.Log("PowerUpSpawner started!");
        InvokeRepeating(nameof(SpawnRandomPowerUp), spawnInterval, spawnInterval);
    }

    void SpawnRandomPowerUp()
    {
        // KIỂM TRA CÁC ĐIỀU KIỆN
        if (spawnPoints.Length == 0)
        {
            Debug.LogError("No spawn points assigned!");
            return;
        }

        if (shieldPowerUp == null || ammoPowerUp == null)
        {
            Debug.LogError("PowerUp prefabs not assigned!");
            return;
        }

        // KIỂM TRA GIỚI HẠN POWERUP TRÊN MÀN HÌNH
        if (currentPowerUpCount >= maxPowerUpsOnScreen)
        {
            Debug.Log("Too many PowerUps on screen, skipping spawn");
            return;
        }

        // Random chọn powerup
        GameObject powerUpToSpawn = Random.Range(0, 2) == 0 ? shieldPowerUp : ammoPowerUp;

        // Random spawn point
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // SPAWN VÀ TRACK

        // ĐẢM BẢO Z = 0 KHI SPAWN
        Vector3 spawnPos = spawnPoint.position;
        spawnPos.z = 0; // Đặt Z = 0 để hiển thị đúng

        GameObject spawnedPowerUp = Instantiate(powerUpToSpawn, spawnPos, Quaternion.identity);
        currentPowerUpCount++;

        // THÊM COMPONENT ĐỂ TRACK KHI POWERUP BỊ DESTROY
        PowerUpTracker tracker = spawnedPowerUp.AddComponent<PowerUpTracker>();
        tracker.spawner = this;

        // DEBUG
        Debug.Log($"Spawned {powerUpToSpawn.name} at {spawnPoint.name}. Count: {currentPowerUpCount}");
    }

    // HÀM ĐƯỢC GỌI KHI POWERUP BỊ DESTROY
    public void OnPowerUpDestroyed()
    {
        currentPowerUpCount--;
        Debug.Log($"PowerUp destroyed. Current count: {currentPowerUpCount}");
    }
}

// SCRIPT HELPER ĐỂ TRACK POWERUP
public class PowerUpTracker : MonoBehaviour
{
    public PowerUpSpawner spawner;

    void OnDestroy()
    {
        if (spawner != null)
        {
            spawner.OnPowerUpDestroyed();
        }
    }
}

using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Obstacle Settings")]
    public GameObject[] obstaclePrefabs; // Array chứa nhiều obstacles
    public float spawnRate = 2.0f;
    public float spawnRangeX = 8.0f; // Giữ lại để fallback
    public float spawnY = 10.0f;

    [Header("Difficulty Settings")]
    public bool increaseDifficulty = true;
    public float difficultyIncreaseRate = 0.01f;

    [Header("Background Integration")]
    public bool useBackgroundBounds = true; // Toggle để bật/tắt
    public float marginFromEdge = 1.0f; // Margin từ biên background

    // Biến tự động tính từ background
    private float minSpawnX;
    private float maxSpawnX;
    private Background background;
    private float nextSpawnTime = 0f;

    void Start()
    {
        // Tìm Background component nếu được bật
        if (useBackgroundBounds)
        {
            background = Object.FindFirstObjectByType<Background>();

            if (background != null)
            {
                // Lấy giới hạn từ background với margin
                minSpawnX = background.MinPoint.x + marginFromEdge;
                maxSpawnX = background.MaxPoint.x - marginFromEdge;

                Debug.Log("Using Background bounds! Spawn range: " + minSpawnX + " to " + maxSpawnX);
            }
            else
            {
                // Fallback về spawnRangeX cũ nếu không tìm thấy background
                minSpawnX = -spawnRangeX;
                maxSpawnX = spawnRangeX;
                Debug.LogWarning("Background not found! Using spawnRangeX: " + spawnRangeX);
            }
        }
        else
        {
            // Sử dụng spawnRangeX cũ nếu không bật useBackgroundBounds
            minSpawnX = -spawnRangeX;
            maxSpawnX = spawnRangeX;
            Debug.Log("Using manual spawnRangeX: " + spawnRangeX);
        }
    }

    void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnRandomObstacle();

            // Tăng độ khó theo thời gian
            float currentSpawnRate = spawnRate;
            if (increaseDifficulty)
            {
                currentSpawnRate = Mathf.Max(0.5f, spawnRate - Time.time * difficultyIncreaseRate);
            }

            nextSpawnTime = Time.time + currentSpawnRate;
        }
    }

    void SpawnRandomObstacle()
    {
        if (obstaclePrefabs.Length == 0) return;

        // Random chọn obstacle từ array
        int randomIndex = Random.Range(0, obstaclePrefabs.Length);
        GameObject selectedObstacle = obstaclePrefabs[randomIndex];

        // Random vị trí spawn (sử dụng minSpawnX và maxSpawnX)
        float randomX = Random.Range(minSpawnX, maxSpawnX);
        Vector3 spawnPosition = new Vector3(randomX, spawnY, 0f);

        // Spawn obstacle
        Instantiate(selectedObstacle, spawnPosition, selectedObstacle.transform.rotation);

        Debug.Log("Spawned: " + selectedObstacle.name + " at X: " + randomX);
    }

    // Method để cập nhật spawn range trong runtime (tùy chọn)
    public void UpdateSpawnRange()
    {
        if (useBackgroundBounds && background != null)
        {
            minSpawnX = background.MinPoint.x + marginFromEdge;
            maxSpawnX = background.MaxPoint.x - marginFromEdge;
        }
        else
        {
            minSpawnX = -spawnRangeX;
            maxSpawnX = spawnRangeX;
        }
    }
}

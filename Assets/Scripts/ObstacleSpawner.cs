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

    [Header("Speed Increase System")]
    public float baseObstacleSpeed = 2f; // Tốc độ cơ bản của obstacles
    public float speedIncreaseRate = 0.5f; // Tăng tốc độ theo thời gian
    public float maxObstacleSpeed = 10f; // Tốc độ tối đa
    public float speedIncreaseInterval = 5f; // Tăng tốc mỗi 10 giây

    [Header("Background Integration")]
    public bool useBackgroundBounds = true; // Toggle để bật/tắt
    public float marginFromEdge = 1.0f; // Margin từ biên background

    // Biến tự động tính từ background
    private float minSpawnX;
    private float maxSpawnX;
    private Background background;
    private float nextSpawnTime = 0f;

    // THÊM: Biến cho hệ thống tăng tốc
    private float currentObstacleSpeed;
    private float gameStartTime;
    private float speedTimer = 0f;

    void Start()
    {
        // THÊM: Khởi tạo hệ thống tốc độ
        currentObstacleSpeed = baseObstacleSpeed;
        gameStartTime = Time.time;

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
        // THÊM: Cập nhật tốc độ obstacles theo thời gian
        UpdateObstacleSpeed();

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

    // THÊM: Hàm cập nhật tốc độ obstacles
    void UpdateObstacleSpeed()
    {
        speedTimer += Time.deltaTime;

        if (speedTimer >= speedIncreaseInterval)
        {
            speedTimer = 0f;
            IncreaseObstacleSpeed();
        }

        // Cập nhật tốc độ cho tất cả obstacles hiện tại
        ApplySpeedToExistingObstacles();
    }

    // THÊM: Tăng tốc độ obstacles
    void IncreaseObstacleSpeed()
    {
        if (currentObstacleSpeed < maxObstacleSpeed)
        {
            float oldSpeed = currentObstacleSpeed;
            currentObstacleSpeed += speedIncreaseRate;
            currentObstacleSpeed = Mathf.Min(currentObstacleSpeed, maxObstacleSpeed);

            Debug.Log($"Obstacle speed increased from {oldSpeed:F2} to {currentObstacleSpeed:F2}");
        }
    }

    // THÊM: Áp dụng tốc độ cho obstacles hiện tại
    void ApplySpeedToExistingObstacles()
    {
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacles");
        foreach (GameObject obstacle in obstacles)
        {
            ObstacleBehaviour obstacleBehaviour = obstacle.GetComponent<ObstacleBehaviour>();
            if (obstacleBehaviour != null)
            {
                obstacleBehaviour.SetSpeed(currentObstacleSpeed);
            }
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
        GameObject spawnedObstacle = Instantiate(selectedObstacle, spawnPosition, selectedObstacle.transform.rotation);

        // THÊM: Set tốc độ cho obstacle mới spawn
        ObstacleBehaviour obstacleBehaviour = spawnedObstacle.GetComponent<ObstacleBehaviour>();
        if (obstacleBehaviour != null)
        {
            obstacleBehaviour.SetSpeed(currentObstacleSpeed);
        }

        Debug.Log($"Spawned: {selectedObstacle.name} at X: {randomX} with speed: {currentObstacleSpeed:F2}");
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

    // THÊM: Getter cho tốc độ hiện tại
    public float GetCurrentObstacleSpeed()
    {
        return currentObstacleSpeed;
    }

    // THÊM: Reset tốc độ (cho restart game)
    public void ResetSpeed()
    {
        currentObstacleSpeed = baseObstacleSpeed;
        speedTimer = 0f;
        gameStartTime = Time.time;
    }

    // THÊM: Debug info
    void OnGUI()
    {
        if (Application.isEditor)
        {
            GUI.Label(new Rect(10, 100, 200, 20), $"Obstacle Speed: {currentObstacleSpeed:F2}");
            GUI.Label(new Rect(10, 120, 200, 20), $"Max Speed: {maxObstacleSpeed:F2}");
        }
    }
}

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class InstructionManager : MonoBehaviour
{
    [Header("UI Elements")]
    public Text instructionText;
    public RectTransform upArrow;
    public RectTransform downArrow;
    public RectTransform leftArrow;
    public RectTransform rightArrow;

    [Header("Animation Prefabs")]
    public GameObject planePrefab;
    public GameObject asteroidPrefab;
    public GameObject bulletPrefab;
    public GameObject explosionPrefab;
    public GameObject starPrefab;

    [Header("Animation Positions")]
    public Transform stageCenter;
    public Transform asteroidSpawnPoint;

    [Header("Animation Settings")]
    public float moveDistance = 2f;
    public float animationSpeed = 2f;

    [Header("Timing")]
    public float instructionDelay = 3f;
    public float loopDelay = 2f;

    [Header("Key Bounce Settings")]
    public float bounceScale = 1.25f;
    public float bounceDuration = 0.1f;

    [Header("Audio")]
    public AudioClip keyPressSound;
    public AudioClip collectSound; // ÂM THANH MỚI CHO VIỆC THU THẬP

    private AudioSource audioSource;
    private Vector3 initialScale = Vector3.one;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) { audioSource = gameObject.AddComponent<AudioSource>(); }
    }

    void Start()
    {
        StartCoroutine(FullInstructionSequence());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow)) { StartCoroutine(BounceKey(upArrow)); }
        else if (Input.GetKeyDown(KeyCode.DownArrow)) { StartCoroutine(BounceKey(downArrow)); }
        else if (Input.GetKeyDown(KeyCode.LeftArrow)) { StartCoroutine(BounceKey(leftArrow)); }
        else if (Input.GetKeyDown(KeyCode.RightArrow)) { StartCoroutine(BounceKey(rightArrow)); }
    }

    private IEnumerator FullInstructionSequence()
    {
        while (true)
        {
            // Giai đoạn 1 (Không đổi)
            instructionText.text = "Dùng các phím mũi tên để di chuyển...";
            GameObject plane = Instantiate(planePrefab, stageCenter.position, Quaternion.identity);
            yield return new WaitForSeconds(1f);
            yield return MoveObject(plane.transform, plane.transform.position + Vector3.left * moveDistance);
            yield return MoveObject(plane.transform, stageCenter.position);
            yield return new WaitForSeconds(instructionDelay);

            // Giai đoạn 2 (Đã được cập nhật)
            instructionText.text = "Bắn thiên thạch để nhận sao...";
            GameObject asteroid = Instantiate(asteroidPrefab, asteroidSpawnPoint.position, Quaternion.identity);
            yield return MoveObject(asteroid.transform, stageCenter.position + Vector3.up * 1.5f);

            GameObject bullet = Instantiate(bulletPrefab, plane.transform.position, Quaternion.identity);
            yield return MoveObject(bullet.transform, asteroid.transform.position);

            Instantiate(explosionPrefab, asteroid.transform.position, Quaternion.identity);
            GameObject star = Instantiate(starPrefab, asteroid.transform.position, Quaternion.identity);

            Destroy(asteroid);
            Destroy(bullet);

            // === PHẦN LOGIC ĐÃ ĐƯỢC SỬA ĐỔI ===
            // 1. Chờ một chút để người chơi thấy ngôi sao
            yield return new WaitForSeconds(0.5f);

            // 2. CHO MÁY BAY TỰ BAY ĐẾN VỊ TRÍ CỦA NGÔI SAO
            yield return MoveObject(plane.transform, star.transform.position);

            // 3. Phát âm thanh thu thập và hủy ngôi sao đi
            if (collectSound != null) { audioSource.PlayOneShot(collectSound); }
            Destroy(star);
            // === KẾT THÚC PHẦN SỬA ĐỔI ===

            Destroy(plane);
            yield return new WaitForSeconds(loopDelay);
            instructionText.text = "";
        }
    }

    private IEnumerator MoveObject(Transform objectToMove, Vector3 targetPosition)
    {
        float timer = 0;
        Vector3 startPosition = objectToMove.position;
        float duration = Vector3.Distance(startPosition, targetPosition) / animationSpeed;
        while (timer < duration)
        {
            if (objectToMove == null) yield break;
            objectToMove.position = Vector3.Lerp(startPosition, targetPosition, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }
        if (objectToMove != null) objectToMove.position = targetPosition;
    }

    private IEnumerator BounceKey(RectTransform keyTransform)
    {
        if (keyPressSound != null) { audioSource.PlayOneShot(keyPressSound); }
        Vector3 targetScale = new Vector3(bounceScale, bounceScale, 1f);
        float timer = 0;
        while (timer < bounceDuration)
        {
            keyTransform.localScale = Vector3.Lerp(initialScale, targetScale, timer / bounceDuration);
            timer += Time.deltaTime; yield return null;
        }
        keyTransform.localScale = targetScale;
        timer = 0;
        while (timer < bounceDuration)
        {
            keyTransform.localScale = Vector3.Lerp(targetScale, initialScale, timer / bounceDuration);
            timer += Time.deltaTime; yield return null;
        }
        keyTransform.localScale = initialScale;
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("Startgame");
    }
}

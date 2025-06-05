using UnityEngine;

public class BackgroundMusicManager : MonoBehaviour
{
    [Header("Music Settings")]
    public AudioSource musicSource;
    public AudioClip menuMusic;
    public AudioClip gameMusic;

    [Header("Volume Settings")]
    public float musicVolume = 0.5f;

    public static BackgroundMusicManager Instance;

    void Awake()
    {
        // SỬA: Cải tiến Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SetupMusicSource();
            Debug.Log("BackgroundMusicManager created and set to DontDestroyOnLoad");
        }
        else
        {
            Debug.Log("BackgroundMusicManager already exists, destroying duplicate");
            Destroy(gameObject);
            return; // SỬA: Thoát sớm để không chạy Start()
        }
    }

    void Start()
    {
        Time.timeScale = 1f;

        // Dừng nhạc hoặc phát nhạc game over
        if (BackgroundMusicManager.Instance != null)
        {
            BackgroundMusicManager.Instance.StopMusic();
            Debug.Log("Music stopped for End Scene");
        }
        // SỬA: Chỉ chạy nếu là Instance chính
        if (Instance == this)
        {
            PlayMenuMusic();
        }
    }

    void SetupMusicSource()
    {
        if (musicSource == null)
        {
            musicSource = gameObject.AddComponent<AudioSource>();
        }

        musicSource.loop = true;
        musicSource.volume = musicVolume;
        musicSource.playOnAwake = false;
        musicSource.mute = false; // SỬA: Đảm bảo không bị mute

        Debug.Log($"Music source setup - Volume: {musicSource.volume}, Loop: {musicSource.loop}");
    }

    public void PlayMenuMusic()
    {
        // SỬA: Thêm debug chi tiết
        Debug.Log($"PlayMenuMusic called. Instance exists: {Instance != null}");
        Debug.Log($"MenuMusic clip: {menuMusic != null}");
        Debug.Log($"MusicSource: {musicSource != null}");

        if (menuMusic != null && musicSource != null)
        {
            musicSource.clip = menuMusic;
            musicSource.volume = musicVolume; // SỬA: Đảm bảo volume đúng
            musicSource.Play();
            Debug.Log($"Menu music playing: {musicSource.isPlaying}, Volume: {musicSource.volume}");
        }
        else
        {
            if (menuMusic == null)
                Debug.LogError("Menu Music clip is NULL! Please assign it in Inspector.");
            if (musicSource == null)
                Debug.LogError("Music AudioSource is NULL!");
        }
    }

    public void PlayGameMusic()
    {
        // SỬA: Thêm debug chi tiết
        Debug.Log($"PlayGameMusic called. Instance exists: {Instance != null}");
        Debug.Log($"GameMusic clip: {gameMusic != null}");

        if (gameMusic != null && musicSource != null)
        {
            musicSource.clip = gameMusic;
            musicSource.volume = musicVolume; // SỬA: Đảm bảo volume đúng
            musicSource.Play();
            Debug.Log($"Game music playing: {musicSource.isPlaying}, Volume: {musicSource.volume}");
        }
        else
        {
            if (gameMusic == null)
                Debug.LogError("Game Music clip is NULL! Please assign it in Inspector.");
            if (musicSource == null)
                Debug.LogError("Music AudioSource is NULL!");
        }
    }

    public void StopMusic()
    {
        if (musicSource != null)
        {
            musicSource.Stop();
            Debug.Log("Music stopped");
        }
    }

    // SỬA: Thêm hàm kiểm tra trạng thái
    public bool IsMusicPlaying()
    {
        return musicSource != null && musicSource.isPlaying;
    }

    // SỬA: Thêm hàm reset nếu cần
    [ContextMenu("Reset Music Manager")]
    void ResetMusicManager()
    {
        if (musicSource != null)
        {
            musicSource.Stop();
            musicSource.volume = musicVolume;
            musicSource.mute = false;
            Debug.Log("Music Manager reset");
        }
    }

    // SỬA: Thêm hàm debug info
    [ContextMenu("Debug Music Info")]
    void DebugMusicInfo()
    {
        Debug.Log("=== MUSIC MANAGER DEBUG ===");
        Debug.Log($"Instance: {Instance != null}");
        Debug.Log($"MusicSource: {musicSource != null}");
        Debug.Log($"Volume: {(musicSource != null ? musicSource.volume.ToString() : "NULL")}");
        Debug.Log($"Is Playing: {(musicSource != null ? musicSource.isPlaying.ToString() : "NULL")}");
        Debug.Log($"Current Clip: {(musicSource != null && musicSource.clip != null ? musicSource.clip.name : "NULL")}");
        Debug.Log($"Menu Music: {(menuMusic != null ? menuMusic.name : "NULL")}");
        Debug.Log($"Game Music: {(gameMusic != null ? gameMusic.name : "NULL")}");
        Debug.Log("========================");
    }

    // SỬA: Đảm bảo không bị destroy khi load scene
    void OnLevelWasLoaded(int level)
    {
        Debug.Log($"Scene loaded: {level}. Music still playing: {IsMusicPlaying()}");

        // Kiểm tra và khôi phục music nếu cần
        if (!IsMusicPlaying() && menuMusic != null)
        {
            Debug.Log("Music stopped after scene load, restarting...");
            PlayMenuMusic();
        }
    }
}

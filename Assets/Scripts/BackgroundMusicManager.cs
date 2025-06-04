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

    // Thêm vào BackgroundMusicManager
    void Start()
    {
        PlayMenuMusic();
    }

    void Awake()
    {
        // Singleton pattern để giữ music qua scenes
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SetupMusicSource();
        }
        else
        {
            Destroy(gameObject);
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
    }

    public void PlayMenuMusic()
    {
        if (menuMusic != null)
        {
            musicSource.clip = menuMusic;
            musicSource.Play();
            Debug.Log("Menu music started");
        }
    }

    public void PlayGameMusic()
    {
        if (gameMusic != null)
        {
            musicSource.clip = gameMusic;
            musicSource.Play();
            Debug.Log("Game music started");
        }
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }
}

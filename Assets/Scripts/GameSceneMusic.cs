using UnityEngine;

public class GameSceneMusic : MonoBehaviour
{
    void Start()
    {
        if (BackgroundMusicManager.Instance != null)
        {
            BackgroundMusicManager.Instance.PlayGameMusic();
        }
    }
}

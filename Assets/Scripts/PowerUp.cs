using UnityEngine;

public enum PowerUpType
{
    Shield,
    Ammo
}

public class PowerUp : MonoBehaviour
{
    [Header("PowerUp Settings")]
    public PowerUpType powerUpType;
    public float shieldDuration = 5f; // Thời gian shield
    public int ammoAmount = 10; // Số đạn tăng thêm

    [Header("Visual Effects")]
    public float rotateSpeed = 50f;

    void Update()
    {
        // Hiệu ứng xoay
        transform.Rotate(0, 0, rotateSpeed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerBehaviour player = other.GetComponent<PlayerBehaviour>();
            if (player != null)
            {
                ApplyPowerUp(player);
                Destroy(gameObject);
            }
        }
    }

    void ApplyPowerUp(PlayerBehaviour player)
    {
        switch (powerUpType)
        {
            case PowerUpType.Shield:
                player.ActivateShield(shieldDuration);
                Debug.Log("Shield activated!");
                break;
            case PowerUpType.Ammo:
                player.ActivateTripleShot(10f); // Triple shot trong 10 giây
                Debug.Log("Triple Shot activated!");
                break;
        }
    }


}

using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public class PlayerBehaviour : MonoBehaviour
{
    public GameObject bulletPrefab;
    float speed = 5.0f;
    private Keyboard keyboard;

    public Vector3 sizeOfSprite
    {
        get {
            return new(this.GetComponent<SpriteRenderer>().sprite.rect.width, this.GetComponent<SpriteRenderer>().sprite.rect.height, 0.0f);
        }
    }
    void Start()
    {
        keyboard = Keyboard.current; // Khởi tạo keyboard
    }

    void Update()
    {
        if (keyboard == null) return;

        if (keyboard.spaceKey.wasPressedThisFrame)
        {
            Fire();
        }

        if (keyboard.wKey.isPressed || keyboard.upArrowKey.isPressed)
        {
            this.transform.position += new Vector3(0.0f, speed * Time.deltaTime, 0.0f);
        }

        if (keyboard.sKey.isPressed || keyboard.downArrowKey.isPressed)
        {
            this.transform.position += Vector3.down * speed * Time.deltaTime;
        }

        if (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed)
        {
            this.transform.position += Vector3.left * speed * Time.deltaTime;
        }

        if (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed)
        {
            this.transform.position += Vector3.right * speed * Time.deltaTime;
        }
    }
    public void Fire()
    {
        GameObject bulletClone = (GameObject)GameObject.Instantiate(bulletPrefab);
        // Calculate position
        //Vector3 position = new Vector3(this.transform.position.x, this.transform.position.y + sizeOfSprite.y / 2.0f - bulletClone.GetComponent<BulletBehaviour>().sizeOfSprite.y / 2.0f, 0.0f);
        //bulletClone.transform.position = position;
    }
}

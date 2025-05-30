using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBehaviour : MonoBehaviour
{
    float speed = 5.0f;
    private Keyboard keyboard;

    void Start()
    {
        keyboard = Keyboard.current;
    }

    void Update()
    {
        if (keyboard == null) return;

        if (keyboard.spaceKey.wasPressedThisFrame)
        {
            // Xử lý khi nhấn Spacessssss
        }

        if (keyboard.wKey.isPressed)
        {
            this.transform.position += Vector3.forward * speed * Time.deltaTime;
        }

        if (keyboard.sKey.isPressed)
        {
            this.transform.position += Vector3.back * speed * Time.deltaTime;
        }

        if (keyboard.aKey.isPressed)
        {
            this.transform.position += Vector3.left * speed * Time.deltaTime;
        }

        if (keyboard.dKey.isPressed)
        {
            this.transform.position += Vector3.right * speed * Time.deltaTime;
        }
    }
}

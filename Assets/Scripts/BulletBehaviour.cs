using UnityEngine;

[System.Serializable]
public class BulletBehaviour: MonoBehaviour
{
    float LimitY;
    float speed = 5.0f;
    [HideInInspector]
    public Vector3 sizeOfSprite;

    void Start()
    {
        sizeOfSprite = new Vector3(this.GetComponent<SpriteRenderer>().sprite.rect.width, this.GetComponent<SpriteRenderer>().sprite.rect.height, 0.0f);
        LimitY = FindObjectOfType<Background>().MaxPoint.y + sizeOfSprite.y / 2.0f;
    }
    void Update()
    {
        if(this.transform.position.y >= LimitY)
        {
            GameObject.Destroy(this.gameObject);
        }
        else
        {
            this.transform.position += new Vector3(0.0f, speed * Time.deltaTime, 0.0f);
        }
    }
}

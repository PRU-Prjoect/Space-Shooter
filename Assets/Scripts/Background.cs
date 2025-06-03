using UnityEngine;

[System.Serializable]
public class Background : MonoBehaviour
{
    [HideInInspector]
    public Vector3 MaxPoint;
    [HideInInspector]
    public Vector3 MinPoint;
    void Start()
    {
        MaxPoint = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight, 0.0f));
        MinPoint = Camera.main.ScreenToWorldPoint(Vector3.zero);
        float WidthScreen = MaxPoint.x - MinPoint.x;
        float HeightScreen = MaxPoint.y - MinPoint.y;
        //
        float pixelPerUnit = this.GetComponent<SpriteRenderer>().sprite.pixelsPerUnit;
        Vector3 sizeOfSprite = new (this.GetComponent<SpriteRenderer>().sprite.rect.width, this.GetComponent<SpriteRenderer>().sprite.rect.height, 0.0f);
        //
        Vector3 sizeOfBackground = new (WidthScreen / (sizeOfSprite.x / pixelPerUnit), HeightScreen / (sizeOfSprite.y / pixelPerUnit), 0.0f);
        this.transform.localScale = sizeOfBackground;
    }

}


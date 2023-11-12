using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextLoader : MonoBehaviour
{
    public float speed;
    public RectTransform maskRec;
    public RectTransform rec;
    float localX;
    float localY;
    float localZ;
    float txtWidth;
    void Start()
    {
        localY = transform.localPosition.y;
        localZ = transform.localPosition.z;
        Debug.LogError(maskRec.rect.width);
        rec.anchoredPosition = new Vector2(maskRec.rect.width, 0);
    }
    void Update()
    {
        if (speed != 0)
        {
            txtWidth = rec.rect.width;
            if (rec.anchoredPosition.x < -txtWidth)
            {
                rec.anchoredPosition = new Vector2(maskRec.rect.width, 0);
            }
            localX = transform.localPosition.x - speed * Time.deltaTime;
            transform.localPosition = new Vector3(localX, localY, localZ);
        }
    }
}

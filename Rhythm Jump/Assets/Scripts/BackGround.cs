using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround : MonoBehaviour
{
    public float speed;
    public int startIndex;
    public int endIndex;
    public Transform[] sprites;

    float viewWidth;

    void Awake()
    {
        viewWidth = 5;
    }

    void Update()
    {
        Vector3 curPos = transform.position;
        Vector3 nextPos = Vector3.left * speed * Time.deltaTime;
        transform.position = curPos + nextPos;

        if (sprites[startIndex].position.x < viewWidth * -2)
        {
            Vector3 backSpritePos = sprites[endIndex].localPosition;
            sprites[startIndex].transform.localPosition = backSpritePos + Vector3.right * viewWidth;

            endIndex = startIndex;
            startIndex = (startIndex + 1) % sprites.Length;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorManager : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    public MapManager mapManager;
    public GameManager gameManager;
    public bool damage;
    float patternTime;

    int i = -1;
    int floorCol;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (i >= 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(i % floorCol, -1 * i / floorCol), Time.deltaTime * 50);
            Invoke("FloorStay", 1f);
        }
    }

    public void FloorStart(int n, int col)
    {
        i = n;
        floorCol = col;
    }

    void FloorStay()
    {
        i = -1;
    }

    public void PatternTime()
    {
        if (mapManager != null)
        {
            patternTime = mapManager.patternTime;
        }
        else
        {
            patternTime = gameManager.patternTime;
        }
    }

    public void floorPatternStart() // 패턴 시작
    {
        spriteRenderer.color = new Color(1, 0, 0);
        Invoke("floorPatternDamage", patternTime);
    }
    void floorPatternDamage() // 데미지를 받는 패턴
    {
        spriteRenderer.color = new Color(0, 0, 0);
        damage = true;
        Invoke("floorPatternEnd", patternTime);
    }
    void floorPatternEnd() // 패턴 끝
    {
        spriteRenderer.color = new Color(1, 1, 1);
        damage = false;
    }
}

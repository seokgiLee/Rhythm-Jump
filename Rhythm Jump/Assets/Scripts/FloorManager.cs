using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorManager : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    public PlayerManager player;
    public MapManager mapManager;
    public bool damage;
    float timeScale;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void TimeScale()
    {
        timeScale = mapManager.timeScale;
    }

    public void floorPatternStart() // 패턴 시작
    {
        spriteRenderer.color = new Color(0, 1, 0);
        Invoke("floorPatternYellow", 2 / timeScale);
    }
    void floorPatternYellow() // 패턴 경고
    {
        spriteRenderer.color = new Color(1, 1, 0);
        Invoke("floorPatternRed", 2 / timeScale);
    }
    void floorPatternRed() // 패턴 경고
    {
        spriteRenderer.color = new Color(1, 0, 0);
        Invoke("floorPatternDamage", 2 / timeScale);
    }
    void floorPatternDamage() // 데미지를 받는 패턴
    {
        spriteRenderer.color = new Color(0, 0, 0);
        damage = true;
        Invoke("floorPatternEnd", 2 / timeScale);
    }
    void floorPatternEnd() // 패턴 끝
    {
        spriteRenderer.color = new Color(1, 1, 1);
        damage = false;
    }
}

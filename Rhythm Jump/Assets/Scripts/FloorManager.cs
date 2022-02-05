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

    public void floorPatternStart() // ���� ����
    {
        spriteRenderer.color = new Color(0, 1, 0);
        Invoke("floorPatternYellow", 2 / timeScale);
    }
    void floorPatternYellow() // ���� ���
    {
        spriteRenderer.color = new Color(1, 1, 0);
        Invoke("floorPatternRed", 2 / timeScale);
    }
    void floorPatternRed() // ���� ���
    {
        spriteRenderer.color = new Color(1, 0, 0);
        Invoke("floorPatternDamage", 2 / timeScale);
    }
    void floorPatternDamage() // �������� �޴� ����
    {
        spriteRenderer.color = new Color(0, 0, 0);
        damage = true;
        Invoke("floorPatternEnd", 2 / timeScale);
    }
    void floorPatternEnd() // ���� ��
    {
        spriteRenderer.color = new Color(1, 1, 1);
        damage = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorManager : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    public PlayerManager player;
    public MapManager mapManager;
    public bool damage;
    float patternTime;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void TimeScale()
    {
        patternTime = mapManager.patternTime;
    }

    public void floorPatternStart() // ���� ����
    {
        spriteRenderer.color = new Color(1, 0, 0);
        Invoke("floorPatternDamage", patternTime);
    }
    void floorPatternDamage() // �������� �޴� ����
    {
        spriteRenderer.color = new Color(0, 0, 0);
        damage = true;
        Invoke("floorPatternEnd", patternTime);
    }
    void floorPatternEnd() // ���� ��
    {
        spriteRenderer.color = new Color(1, 1, 1);
        damage = false;
    }
}

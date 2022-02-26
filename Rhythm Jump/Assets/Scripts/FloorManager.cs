using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorManager : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    public MapManager mapManager;
    public GameManager gameManager;
    public TutorialManager tutorialManager;
    public bool damage;
    float patternTime;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void FloorStart(int i, int floorCol)
    {
        transform.DOMove(new Vector3(i % floorCol, -1 * i / floorCol), 0.5f).SetEase(Ease.Linear);
    }

    public void PatternTime()
    {
        if (mapManager != null)
        {
            patternTime = mapManager.patternTime;
        }
        else if(gameManager != null)
        {
            patternTime = gameManager.patternTime;
        }
        else
        {
            patternTime = tutorialManager.patternTime;
        }
    }

    public void floorPatternStart() // 패턴 시작
    {
        spriteRenderer.DOColor(new Color(1, 0, 0), patternTime);
        Invoke("floorPatternDamage", patternTime);
    }
    void floorPatternDamage() // 데미지를 받는 패턴
    {
        spriteRenderer.DOColor(new Color(0.3f, 0.3f, 0.3f), patternTime / 2);
        damage = true;
        Invoke("floorPatternEnd", patternTime);
    }
    void floorPatternEnd() // 패턴 끝
    {
        spriteRenderer.DOColor(new Color(1, 1, 1), patternTime / 4);
        damage = false;
    }
}

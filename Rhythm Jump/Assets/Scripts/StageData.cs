using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageData : MonoBehaviour
{
    public int floorRow; // 맵 행
    public int floorCol; // 맵 열
    public float patternTime; // 리듬 주기
    public float patternAccuracy; // 리듬 정확도
    public int[] patternNums; // 스테이지 패턴

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void StageDataMake(int r,int c, float t,float a, int[] n)
    {
        floorRow = r;
        floorCol = c;
        patternTime = t;
        patternAccuracy = a;
        patternNums = n;
    }
}

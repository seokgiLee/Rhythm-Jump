using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageData
{
    public int stageNum; // 현재 스테이지 번호
    public int cutLine; // 클리어 커트라인
    public int floorRow; // 맵 행
    public int floorCol; // 맵 열
    public float patternTime; // 리듬 주기
    public float patternAccuracy; // 리듬 정확도
    public int[] patternNums; // 스테이지 패턴

    public StageData(int s,int l, int r,int c, float t,float a, int[] n)
    {
        stageNum = s;
        cutLine = l;
        floorRow = r;
        floorCol = c;
        patternTime = t;
        patternAccuracy = a;
        patternNums = n;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageData
{
    public int stageNum; // ���� �������� ��ȣ
    public int cutLine; // Ŭ���� ĿƮ����
    public int floorRow; // �� ��
    public int floorCol; // �� ��
    public float patternTime; // ���� �ֱ�
    public float patternAccuracy; // ���� ��Ȯ��
    public int[] patternNums; // �������� ����
    public float bgmStartTime; // BGM ���۽ð�

    public StageData(int s,int l, int r,int c, float t,float a, int[] n, float b)
    {
        stageNum = s;
        cutLine = l;
        floorRow = r;
        floorCol = c;
        patternTime = t;
        patternAccuracy = a;
        patternNums = n;
        bgmStartTime = b;
    }
}

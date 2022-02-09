using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageData : MonoBehaviour
{
    public int floorRow; // �� ��
    public int floorCol; // �� ��
    public float patternTime; // ���� �ֱ�
    public float patternAccuracy; // ���� ��Ȯ��
    public int[] patternNums; // �������� ����

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

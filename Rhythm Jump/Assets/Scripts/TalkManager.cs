using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkManager : MonoBehaviour
{
    Dictionary<int, string[]> talkData;

    void Awake()
    {
        talkData = new Dictionary<int, string[]>();
        GenerateData();
    }

    void GenerateData()
    {
        talkData.Add(1, new string[] { "���������� ���Ű���\nȯ���մϴ�.", "�� �״�� ���뿡 ���缭\n�����Ͻø� �˴ϴ�.", "�ѹ� ������ �����?", "Ʃ�丮���� �׸��ΰ� �����ø�\n������ ��ư�� �����ּ���" });

    }

    public string GetTalk(int id, int talkIndex)
    {
        if (talkIndex == talkData[id].Length) // ��ȭ ��
        {
            return null;
        }
        else
            return talkData[id][talkIndex];
    }
}
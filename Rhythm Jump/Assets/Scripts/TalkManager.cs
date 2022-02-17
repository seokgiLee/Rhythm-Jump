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
        talkData.Add(1, new string[] { "���������� ���Ű���\nȯ���մϴ�.", "�� �״�� ���뿡 ���缭\n�����Ͻø� �˴ϴ�.",
            "�� �ʿ����� 2�ʸ��� �����Ͻø� �˴ϴ�.", "���� �Ʒ��� �ִ� ����Ű�� �����ø� �����մϴ�.",
            "��ư�� 0.5�ʸ��� �����̴ϱ�\n�����Ͻø� �����ϴ�.", "���뿡 ���缭 �������� ������\n�Ǽ� Ƚ���� �����մϴ�.",
            "�ѹ� ������ �����?", "Ʃ�丮���� �׸��ΰ� �����ø�\n������ ��ư�� �����ּ���" });
        talkData.Add(2, new string[] { "���ϼ̾��!", "���� �������\n������ �ѹ� �غ����?",
            "�ٴ��� ������ ���ϸ�\n��õ� �����մϴ�."});
        talkData.Add(3, new string[] { "�����ϴ� ���� ���� ������ �ȵǰ���?", "�׷� �����ϰڽ��ϴ�." });
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
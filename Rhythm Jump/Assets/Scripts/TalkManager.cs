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
            "��ư�� 0.5�ʸ��� �����̴ϱ�\n�����Ͻø� �����ϴ�.", "���뿡 ���缭\n�������� ������\n�Ǽ� Ƚ���� �����մϴ�.",
            "�Ǽ��� �ʹ� ������\n���������� �Ϸ��� �� �����ϴ�.",
            "�ѹ� ������ �����?", "Ʃ�丮����\n�׸��ΰ� �����ø�\n������ ��ư�� �����ּ���" });
        talkData.Add(2, new string[] { "���ϼ̾��!", "���� �������\n������ �ѹ� �غ����?",
            "�ٴ��� ������ ���ϸ�\n��õ� �����մϴ�."});
        talkData.Add(3, new string[] { "�����ϴ� ���� ���� ������\n�� �ǰ���?", "�׷� �����ϰڽ��ϴ�." });
        talkData.Add(4, new string[] { "����ϳ׿�!", "���ڸ� ���� �� ���߼���","���� ����������\n������ ������ ����!"});
        talkData.Add(5, new string[] { "������ �ʱ���!", "���� �Ƿ��� �ðھ��", "���� ����������\n������ ������ ����!" });
        talkData.Add(6, new string[] { "ó���̶�\n���� ����̳� ���׿�","������ ����\n���Ͻð� �� �ſ���", "���� ����������\n������ ������ ����!" });
        talkData.Add(7, new string[] { "......","ó������\n�� �� ���� ����","���� ���ݾ�\n�ͼ������� ���ݾƿ�?", "���� ����������\n������ ������ ����!" });
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
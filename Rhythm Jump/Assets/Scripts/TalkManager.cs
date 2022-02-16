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
        talkData.Add(1, new string[] { "리듬점프에 오신것을\n환영합니다.", "말 그대로 리듬에 맞춰서\n점프하시면 됩니다.", "한번 연습해 볼까요?", "튜토리얼을 그만두고 싶으시면\n나가기 버튼을 눌러주세요" });

    }

    public string GetTalk(int id, int talkIndex)
    {
        if (talkIndex == talkData[id].Length) // 대화 끝
        {
            return null;
        }
        else
            return talkData[id][talkIndex];
    }
}
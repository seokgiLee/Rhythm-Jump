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
        talkData.Add(1, new string[] { "리듬점프에 오신것을\n환영합니다.", "말 그대로 리듬에 맞춰서\n점프하시면 됩니다.",
            "이 맵에서는 2초마다 점프하시면 됩니다.", "왼쪽 아래에 있는 방향키를 누르시면 점프합니다.",
            "버튼은 0.5초마다 움직이니까\n참고하시면 좋습니다.", "리듬에 맞춰서 점프하지 않으면\n실수 횟수가 증가합니다.",
            "한번 연습해 볼까요?", "튜토리얼을 그만두고 싶으시면\n나가기 버튼을 눌러주세요" });
        talkData.Add(2, new string[] { "잘하셨어요!", "이제 맛보기로\n게임을 한번 해볼까요?",
            "바닥이 빨갛게 변하면\n잠시뒤 폭발합니다."});
        talkData.Add(3, new string[] { "폭발하는 발판 위에 있으면 안되겠죠?", "그럼 시작하겠습니다." });
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
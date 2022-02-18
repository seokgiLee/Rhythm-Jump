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
            "버튼은 0.5초마다 움직이니까\n참고하시면 좋습니다.", "리듬에 맞춰서\n점프하지 않으면\n실수 횟수가 증가합니다.",
            "실수가 너무 많으면\n스테이지를 완료할 수 없습니다.",
            "한번 연습해 볼까요?", "튜토리얼을\n그만두고 싶으시면\n나가기 버튼을 눌러주세요" });
        talkData.Add(2, new string[] { "잘하셨어요!", "이제 맛보기로\n게임을 한번 해볼까요?",
            "바닥이 빨갛게 변하면\n잠시뒤 폭발합니다."});
        talkData.Add(3, new string[] { "폭발하는 발판 위에 있으면\n안 되겠죠?", "그럼 시작하겠습니다." });
        talkData.Add(4, new string[] { "대단하네요!", "박자를 정말 잘 맞추세요","이제 본격적으로\n게임을 시작해 봐요!"});
        talkData.Add(5, new string[] { "나쁘지 않군요!", "점점 실력이 늘겠어요", "이제 본격적으로\n게임을 시작해 봐요!" });
        talkData.Add(6, new string[] { "처음이라\n조금 힘드셨나 보네요","앞으로 더욱\n잘하시게 될 거예요", "이제 본격적으로\n게임을 시작해 봐요!" });
        talkData.Add(7, new string[] { "......","처음부터\n잘 할 수는 없죠","원래 조금씩\n익숙해지는 거잖아요?", "이제 본격적으로\n게임을 시작해 봐요!" });
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject cameraPosition;
    public GameObject playerPosition;
    public GameObject[] floorsPosition;
    public FloorManager[] floors;
    public PlayerManager playerManager;

    public Button[] buttons;
    public Animator[] countDowns;
    public GameObject stageTexts;

    public Animator[] ButtonAnimators;

    public bool start; // 맵, 플레이어 등장

    public bool buttonOn; // 버튼을 누르는 타이밍 여부
    public bool buttonClick; // 버튼을 눌렀는지 여부

    public bool playerMove; // 패턴없이 이동만 시작
    public int maxStage; // 진행가능한 가장 높은 스테이지
    public int curStage; // 방금 완료한 스테이지
    public int playerX; // 플레이어 위치
    public int playerY;
    public float stageTextX; // 스테이지 번호 위치
    public float stageTextY;

    public int floorRow; // 발판의 행
    public int floorCol; // 발판의 열
    public int floorNum; // 플레이어가 밟은 발판 번호

    public float time; // 발판패턴용 시간
    public float patternTime; // 패턴주기
    public float patternAccuracy; // 박자 정확도
    public bool animationHint; // 박자 힌트 여부 (이동버튼)

    public Text errorText;
    public int errorCount; // 틀린 횟수

    public bool patternStart; // 패턴 시작
    public bool nextPattern; // 다음패턴 시작여부
    public int[] patternNums; // 패턴순서 모음
    public int curPatternNum; // 현재 패턴순서
    public int pattern; // 현재 패턴번호
    public int startNum; // 패턴 시작발판
    public int small; // 테두리 개수

    void Awake()
    {
        errorText.text = errorCount.ToString();
        playerX = (curStage - 1) % floorRow;
        playerY = -1 * ((curStage - 1) / floorRow);
        floorNum = playerX + -1 * playerY * floorCol;
        cameraPosition.transform.position = new Vector3(playerX, playerY, -10);
        playerPosition.transform.position = new Vector3(playerX, playerY + 10);
        stageTextX = stageTexts.transform.position.x;
        stageTextY = stageTexts.transform.position.y;

        /*
        for (int i = 0; i < floorRow * floorCol; i++)
        {
            floorsPosition[i].SetActive(true);
            floorsPosition[i].transform.position = new Vector3(i % floorCol, -1 * i / floorCol + 10);
            floors[i].PatternTime();
        }*/

        if (floorRow > floorCol)
        {
            small = (floorCol + 1) / 2;
        }
        else
        {
            small = (floorRow + 1) / 2;
        }

        Invoke("MapStart", 1);
    }

    void Update()
    {
        time += Time.deltaTime;

        if (start)
        {
            if (time > 0.5f) // 플레이어 등장
            {
                playerManager.PlayerPosition(playerX, playerY);
                start = false;
                //Invoke("CountDown3", 0.5f);
                Invoke("PlayerMoveStart", 1f);
            }
        }

        if(playerMove)
        {
            if (!buttonClick && !buttonOn)
            {
                Debug.Log("버튼 활성화");
                buttonOn = true;
            }

            if(buttonClick && time>patternTime)
            {
                buttonClick = false;
                time -= patternTime;
            }

            if (time > patternTime * (2 - patternAccuracy))
            {
                ErrorCount();
                time -= patternTime;
                buttonOn = false;
            }
        }

        if (patternStart)
        {
            // 정해진 시간에 도달
            if (time > patternTime * (1 - patternAccuracy) && !buttonClick && !buttonOn)
            {
                // 버튼 활성화
                Debug.Log("버튼 활성화");
                buttonOn = true;
            }
            // 정해진 시간초과
            else if (time > patternTime * patternAccuracy && time < patternTime * (1 - patternAccuracy) && buttonOn)
            {
                Debug.Log("시간초과");
                ErrorCount();
                buttonOn = false;
            }

            if (floors[floorNum].damage) // 플레이어가 검은 발판을 밟으면 데미지
            {
                Debug.Log("검은 발판");
                ErrorCount();
                floors[floorNum].damage = false;
            }          

            if (nextPattern)
            {
                nextPattern = false;
                if (curPatternNum >= patternNums.Length)
                {
                    // 맵 종료
                    pattern = 0;
                    patternStart = false;

                    for (int i = 0; i < buttons.Length; i++)
                    {
                        buttons[i].interactable = false;
                    }

                    for (int i = 0; i < ButtonAnimators.Length; i++)
                    {
                        ButtonAnimators[i].SetTrigger("isStart");
                    }
                }
                else
                {
                    pattern = patternNums[curPatternNum++];
                }

                if (pattern < 3)
                {
                    startNum = 0;
                }
                else if (pattern < 5)
                {
                    startNum = floorRow * floorCol - 1;
                }
                else if (pattern < 6)
                {
                    startNum = 0;
                }
                else if (pattern < 7)
                {
                    startNum = floorCol - 1;
                }
                else if (pattern < 8)
                {
                    startNum = 0;
                }
                else if (pattern < 9)
                {
                    startNum = (floorRow - 1) * floorCol;
                }
                else if (pattern < 10)
                {
                    startNum = 0;
                }
                else if (pattern < 11)
                {
                    startNum = small;
                }
                else if (pattern < 12)
                {
                    startNum = 1;
                }
            }

            switch (pattern)
            {
                case 0: // 공백타임
                    if (time > patternTime)
                    {
                        time -= patternTime;
                        nextPattern = true;
                        if (buttonClick)
                        {
                            buttonClick = false;
                        }
                    }
                    break;
                case 1: // 처음부터 끝까지 가로 순서대로
                    if (time > patternTime)
                    {
                        floorPattern(startNum++);
                        time -= patternTime;
                        if (buttonClick)
                        {
                            buttonClick = false;
                        }
                    }
                    if (startNum >= floorRow * floorCol)
                    {
                        nextPattern = true;
                    }
                    break;
                case 2: // 처음부터 끝까지 세로 순서대로
                    if (time > patternTime)
                    {
                        floorPattern(startNum);
                        startNum += floorCol;
                        if (startNum >= floorRow * floorCol && startNum < (floorRow + 1) * floorCol - 1)
                        {
                            startNum = (startNum + 1) % floorCol;
                        }
                        time -= patternTime;
                        if (buttonClick)
                        {
                            buttonClick = false;
                        }
                    }
                    if (startNum >= floorRow * floorCol)
                    {
                        nextPattern = true;
                    }
                    break;
                case 3: // 끝부터 처음까지 가로 순서대로
                    if (time > patternTime)
                    {
                        floorPattern(startNum--);
                        time -= patternTime;
                        if (buttonClick)
                        {
                            buttonClick = false;
                        }
                    }
                    if (startNum < 0)
                    {
                        nextPattern = true;
                    }
                    break;
                case 4: // 끝부터 처음까지 세로 순서대로
                    if (time > patternTime)
                    {
                        floorPattern(startNum);
                        startNum -= floorCol;
                        if (startNum < 0 && startNum > -1 * floorCol)
                        {
                            startNum = (startNum - 1) + floorRow * floorCol;
                        }
                        time -= patternTime;
                        if (buttonClick)
                        {
                            buttonClick = false;
                        }
                    }
                    if (startNum < 0)
                    {
                        nextPattern = true;
                    }
                    break;
                case 5: // 세로줄 왼쪽 순서대로
                    if (time > patternTime)
                    {
                        for (int i = 0; i < floorRow; i++)
                        {
                            floorPattern(startNum + i * floorCol);
                        }
                        startNum += 1;
                        time -= patternTime;
                        if (buttonClick)
                        {
                            buttonClick = false;
                        }
                    }
                    if (startNum >= floorCol - 1)
                    {
                        nextPattern = true;
                    }
                    break;
                case 6: // 세로줄 오른쪽 순서대로
                    if (time > patternTime)
                    {
                        for (int i = 0; i < floorRow; i++)
                        {
                            floorPattern(startNum + i * floorCol);
                        }
                        startNum -= 1;
                        time -= patternTime;
                        if (buttonClick)
                        {
                            buttonClick = false;
                        }
                    }
                    if (startNum < 1)
                    {
                        nextPattern = true;
                    }
                    break;
                case 7: // 가로줄 위 순서대로
                    if (time > patternTime)
                    {
                        for (int i = 0; i < floorCol; i++)
                        {
                            floorPattern(startNum + i);
                        }
                        startNum += floorCol;
                        time -= patternTime;
                        if (buttonClick)
                        {
                            buttonClick = false;
                        }
                    }
                    if (startNum / floorCol >= floorRow - 1)
                    {
                        nextPattern = true;
                    }
                    break;
                case 8: // 가로줄 아래 순서대로
                    if (time > patternTime)
                    {
                        for (int i = 0; i < floorCol; i++)
                        {
                            floorPattern(startNum + i);
                        }
                        startNum -= floorCol;
                        time -= patternTime;
                        if (buttonClick)
                        {
                            buttonClick = false;
                        }
                    }
                    if (startNum < 1)
                    {
                        nextPattern = true;
                    }
                    break;
                case 9: // 가장자리에서 순서대로
                    if (time > patternTime)
                    {
                        for (int i = 0; i < floorRow * floorCol; i++)
                        {
                            if ((i / floorCol >= startNum && i / floorCol < floorRow - startNum
                                && i % floorCol >= startNum && i % floorCol < floorCol - startNum)
                                && (i % floorCol == startNum || i % floorCol == floorCol - 1 - startNum
                                || i < floorCol * (1 + startNum) || (i > floorCol * (floorRow - 1 - startNum))))
                            {
                                floorPattern(i);
                            }
                        }
                        startNum++;
                        time -= patternTime;
                        if (buttonClick)
                        {
                            buttonClick = false;
                        }
                    }
                    if (startNum > small)
                    {
                        nextPattern = true;
                    }
                    break;
                case 10:  // 가운데에서 순서대로
                    if (time > patternTime)
                    {
                        for (int i = 0; i < floorRow * floorCol; i++)
                        {
                            if ((i / floorCol >= startNum && i / floorCol < floorRow - startNum
                                && i % floorCol >= startNum && i % floorCol < floorCol - startNum)
                                && (i % floorCol == startNum || i % floorCol == floorCol - 1 - startNum
                                || i < floorCol * (1 + startNum) || (i > floorCol * (floorRow - 1 - startNum))))
                            {
                                floorPattern(i);
                            }
                        }
                        startNum--;
                        time -= patternTime;
                        if (buttonClick)
                        {
                            buttonClick = false;
                        }
                    }
                    if (startNum < 0)
                    {
                        nextPattern = true;
                    }
                    break;
                case 11: // 홀짝
                    if (time > patternTime)
                    {
                        for (int i = 0; i < floorRow * floorCol; i++)
                        {
                            if (i % 2 == startNum % 2)
                            {
                                floorPattern(i);
                            }
                        }
                        startNum++;
                        time -= patternTime;
                        if (buttonClick)
                        {
                            buttonClick = false;
                        }
                    }
                    if (startNum > 10)
                    {
                        nextPattern = true;
                    }
                    break;
            }
        }
    }

    public void floorPattern(int n) // 발판 패턴 시작
    {
        floors[n].floorPatternStart();
    }

    public void MoveButton(int n)
    {
        if (buttonOn) // 버튼을 누르는 타이밍
        {
            buttonOn = false;
            buttonClick = true;
            switch (n)
            {
                case 0:
                    if (floorNum - floorCol < 0)
                    {
                        Debug.Log("방향 틀림");
                        ErrorCount();
                    }
                    else
                    {
                        playerY += 1;
                        floorNum -= floorCol;
                        stageTextY -= 131;
                    }
                    break;
                case 1:
                    if (floorNum + floorCol >= floorRow * floorCol)
                    {
                        Debug.Log("방향 틀림");
                        ErrorCount();
                    }
                    else
                    {
                        playerY -= 1;
                        floorNum += floorCol;
                        stageTextY += 131;
                    }
                    break;
                case 2:
                    if (floorNum % floorCol == 0)
                    {
                        Debug.Log("방향 틀림");
                        ErrorCount();
                    }
                    else
                    {
                        playerX -= 1;
                        floorNum -= 1;
                        stageTextX += 131;
                    }
                    break;
                case 3:
                    if (floorNum % floorCol == floorCol - 1)
                    {
                        Debug.Log("방향 틀림");
                        ErrorCount();
                    }
                    else
                    {
                        playerX += 1;
                        floorNum += 1;
                        stageTextX -= 131;
                    }
                    break;
            }
            cameraPosition.transform.position = new Vector3(playerX, playerY, -10);
            playerPosition.transform.position = new Vector3(playerX, playerY);
            stageTexts.transform.position = new Vector3(stageTextX, stageTextY);
        }
        else // 버튼을 누르는 타이밍이 아님
        {
            Debug.Log("타이밍 틀림");
            ErrorCount();
        }
    }

    void MapStart()
    {
        start = true;
        time = 0;
    }

    public void ErrorCount() // 틀린 횟수
    {
        if (buttons[0].interactable)
        {
            errorCount++;
            errorText.text = errorCount.ToString();
        }
    }

    void CountDown3()
    {
        countDowns[2].SetTrigger("isStart");
        Invoke("CountDown2", 1f);
    }
    void CountDown2()
    {
        countDowns[1].SetTrigger("isStart");
        Invoke("CountDown1", 1f);
    }
    void CountDown1()
    {
        countDowns[0].SetTrigger("isStart");
        Invoke("PatternStart", 1f);
    }
    void PatternStart() // 패턴시작
    {
        Debug.Log("시작");
        playerManager.PlayerStart(false);
        patternStart = true;
        nextPattern = true;
        time = 0;

        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].interactable = true;
        }
    }

    void PlayerMoveStart() // 패턴없으 박자에 맞춰서 이동만
    {
        playerManager.PlayerStart(false);
        playerMove = true;
        time = 0;

        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].interactable = true;
        }
        for (int i = 0; i < ButtonAnimators.Length; i++)
        {
            ButtonAnimators[i].SetTrigger("isStart");
        }
    }
}

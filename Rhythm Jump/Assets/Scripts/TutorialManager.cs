using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public GameObject cameraPosition;
    public GameObject playerPosition;
    public GameObject[] floorsPosition;
    public FloorManager[] floors;
    public PlayerManager playerManager;

    public GameObject pause;
    public float backButtonTime; // 스마트폰 뒤로가기 버튼용 타이머
    public bool backButton; // 스마트폰 뒤로가기 버튼 클릭가능 여부

    public Button[] buttons;
    public Animator[] countDowns;

    public Animator[] ButtonAnimators;

    public bool start; // 맵, 플레이어 등장

    public bool buttonOn; // 버튼을 누르는 타이밍 여부
    public bool buttonClick; // 버튼을 눌렀는지 여부

    public int playerX; // 플레이어 위치
    public int playerY;

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

    public TalkManager talkManager;
    public GameObject talkPanel;
    public Text talkText;
    public int talkID;
    public int talkIndex;

    public GameObject jumpSucess;
    public Text jumpSucessText;
    bool tutorial1; // 첫번째 튜토리얼 시작
    public int jumpSucessCount; // 남은 점프 성공횟수
    bool tutorial2; // 두번째 튜토리얼 시작

    void Awake()
    {
        errorText.text = errorCount.ToString();
        playerX = (floorCol - 1) / 2;
        playerY = -1 * (floorRow - 1) / 2;
        floorNum = playerX + -1 * playerY * floorCol;
        cameraPosition.transform.position = new Vector3(playerX, playerY, -10);
        playerPosition.transform.position = new Vector3(playerX, playerY + 10);

        for (int i = 0; i < floorRow * floorCol; i++)
        {
            floors[i].PatternTime();
        }

        if (floorRow > floorCol)
        {
            small = (floorCol + 1) / 2;
        }
        else
        {
            small = (floorRow + 1) / 2;
        }

        Invoke("PlayerAppear", 1);
    }

    void Update()
    {
        if (!backButton)
        {
            backButtonTime += Time.deltaTime;
            if (backButtonTime > 0.5f) // 0.5초마다 클릭가능
            {
                backButton = true;
                time = 0;
            }
        }

        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Escape) && backButton) // 뒤로가기
            {
                if (pause.activeSelf) // 일시정지창이 켜져있으면 계속하기
                {
                    ContinueButton();
                }
                else // 일시정지창이 꺼져있으면 일시정지
                {
                    PauseButton();
                }
                backButton = false;
            }
        }

        time += Time.deltaTime;

        if (tutorial1) // 튜토리얼1 시작
        {
            if (time > patternTime * (1 - patternAccuracy) && !buttonClick && !buttonOn)
            {
                Debug.Log("버튼 활성화");
                buttonOn = true;
            }

            else if (buttonClick && time > patternTime)
            {
                buttonClick = false;
                time -= patternTime;
            }

            else if (time > patternTime * (1 + patternAccuracy))
            {
                ErrorCount();
                time -= patternTime;
                buttonOn = false;
            }

            if (jumpSucessCount < 1) // 튜토리얼1 종료
            {
                tutorial1 = false;
                Invoke("Tutorial1_End", 1f / (2 / patternTime));
            }
        }

        if (tutorial2)
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

                FloorDamage();
            }

            if (nextPattern)
            {
                nextPattern = false;
                if (curPatternNum >= patternNums.Length)
                {
                    // 맵 종료
                    pattern = 0;
                    patternStart = false;

;                   PlayerPrefs.SetInt("Max Stage", 1);
                    ButtonOff();

                    if (errorCount <= 10)
                    {
                        PauseGame(4);
                    }
                    else if (errorCount <= 50)
                    {
                        PauseGame(5);
                    }
                    else if (errorCount <= 100)
                    {
                        PauseGame(6);
                    }
                    else
                    {
                        PauseGame(7);
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
                        playerPosition.transform.DOMoveY(playerY + 0.4f, 0.125f / (2 / patternTime)).SetEase(Ease.OutCubic);
                        playerPosition.transform.DOScale(new Vector3(4f, 4f, 0), 0.125f / (2 / patternTime)).SetEase(Ease.OutCubic);
                        playerPosition.transform.DOMoveY(playerY, 0.125f / (2 / patternTime)).SetEase(Ease.InCubic).SetDelay(0.125f / (2 / patternTime));
                        playerPosition.transform.DOScale(new Vector3(3f, 3f, 0), 0.125f / (2 / patternTime)).SetEase(Ease.InCubic).SetDelay(0.125f / (2 / patternTime));
                        ErrorCount();
                    }
                    else
                    {
                        playerY += 1;
                        floorNum -= floorCol;
                        playerPosition.transform.DOMoveY(playerY - 0.25f, 0.5f / (2 / patternTime)).SetEase(Ease.OutCubic);
                        playerPosition.transform.DOScale(new Vector3(5f, 5f, 0), 0.5f / (2 / patternTime)).SetEase(Ease.OutCubic);
                        playerPosition.transform.DOMoveY(playerY, 0.5f / (2 / patternTime)).SetEase(Ease.InCubic).SetDelay(0.5f / (2 / patternTime));
                        playerPosition.transform.DOScale(new Vector3(3f, 3f, 0), 0.5f / (2 / patternTime)).SetEase(Ease.InCubic).SetDelay(0.5f / (2 / patternTime));
                        jumpSucessCount--;
                        jumpSucessText.text = "점프를 " + jumpSucessCount.ToString() + "번 하세요";
                    }
                    break;
                case 1:
                    if (floorNum + floorCol >= floorRow * floorCol)
                    {
                        Debug.Log("방향 틀림");
                        playerPosition.transform.DOMoveY(playerY - 0.4f, 0.125f / (2 / patternTime)).SetEase(Ease.OutCubic);
                        playerPosition.transform.DOScale(new Vector3(4f, 4f, 0), 0.125f / (2 / patternTime)).SetEase(Ease.OutCubic);
                        playerPosition.transform.DOMoveY(playerY, 0.125f / (2 / patternTime)).SetEase(Ease.InCubic).SetDelay(0.125f / (2 / patternTime));
                        playerPosition.transform.DOScale(new Vector3(3f, 3f, 0), 0.125f / (2 / patternTime)).SetEase(Ease.InCubic).SetDelay(0.125f / (2 / patternTime));
                        ErrorCount();
                    }
                    else
                    {
                        playerY -= 1;
                        floorNum += floorCol;
                        playerPosition.transform.DOMoveY(playerY + 0.5f, 0.5f / (2 / patternTime)).SetEase(Ease.OutCubic);
                        playerPosition.transform.DOScale(new Vector3(5f, 5f, 0), 0.5f / (2 / patternTime)).SetEase(Ease.OutCubic);
                        playerPosition.transform.DOMoveY(playerY, 0.5f / (2 / patternTime)).SetEase(Ease.InCubic).SetDelay(0.5f / (2 / patternTime));
                        playerPosition.transform.DOScale(new Vector3(3f, 3f, 0), 0.5f / (2 / patternTime)).SetEase(Ease.InCubic).SetDelay(0.5f / (2 / patternTime));
                        jumpSucessCount--;
                        jumpSucessText.text = "점프를 " + jumpSucessCount.ToString() + "번 하세요";
                    }
                    break;
                case 2:
                    if (floorNum % floorCol == 0)
                    {
                        Debug.Log("방향 틀림");
                        playerPosition.transform.DOMoveX(playerX - 0.25f, 0.25f / (2 / patternTime)).SetEase(Ease.Linear);
                        playerPosition.transform.DOMoveY(playerY + 0.25f, 0.125f / (2 / patternTime)).SetEase(Ease.OutCubic);
                        playerPosition.transform.DOMoveX(playerX, 0.125f / (2 / patternTime)).SetEase(Ease.Linear).SetDelay(0.125f / (2 / patternTime));
                        playerPosition.transform.DOMoveY(playerY, 0.125f / (2 / patternTime)).SetEase(Ease.InCubic).SetDelay(0.125f / (2 / patternTime));
                        ErrorCount();
                    }
                    else
                    {
                        playerX -= 1;
                        floorNum -= 1;
                        playerPosition.transform.DOMoveX(playerX, 1f / (2 / patternTime)).SetEase(Ease.Linear);
                        playerPosition.transform.DOMoveY(playerY + 0.5f, 0.5f / (2 / patternTime)).SetEase(Ease.OutQuart);
                        playerPosition.transform.DOMoveY(playerY, 0.5f / (2 / patternTime)).SetEase(Ease.InQuart).SetDelay(0.5f / (2 / patternTime));
                        jumpSucessCount--;
                        jumpSucessText.text = "점프를 " + jumpSucessCount.ToString() + "번 하세요";
                    }
                    break;
                case 3:
                    if (floorNum % floorCol == floorCol - 1)
                    {
                        Debug.Log("방향 틀림");
                        playerPosition.transform.DOMoveX(playerX + 0.25f, 0.125f / (2 / patternTime)).SetEase(Ease.Linear);
                        playerPosition.transform.DOMoveY(playerY + 0.25f, 0.125f / (2 / patternTime)).SetEase(Ease.OutCubic);
                        playerPosition.transform.DOMoveX(playerX, 0.125f / (2 / patternTime)).SetEase(Ease.Linear).SetDelay(0.125f / (2 / patternTime));
                        playerPosition.transform.DOMoveY(playerY, 0.125f / (2 / patternTime)).SetEase(Ease.InCubic).SetDelay(0.125f / (2 / patternTime));
                        ErrorCount();
                    }
                    else
                    {
                        playerX += 1;
                        floorNum += 1;
                        playerPosition.transform.DOMoveX(playerX, 1f / (2 / patternTime)).SetEase(Ease.Linear);
                        playerPosition.transform.DOMoveY(playerY + 0.5f, 0.5f / (2 / patternTime)).SetEase(Ease.OutQuart);
                        playerPosition.transform.DOMoveY(playerY, 0.5f / (2 / patternTime)).SetEase(Ease.InQuart).SetDelay(0.5f / (2 / patternTime));
                        jumpSucessCount--;
                        jumpSucessText.text = "점프를 " + jumpSucessCount.ToString() + "번 하세요";
                    }
                    break;
            }
            cameraPosition.transform.DOMove(new Vector3(playerX, playerY, -10), 1f / (2 / patternTime));
            Invoke("FloorDamage", 1f / (2 / patternTime));
        }
        else // 버튼을 누르는 타이밍이 아님
        {
            Debug.Log("타이밍 틀림");
            ErrorCount();
        }
    }

    void FloorDamage()
    {
        if (floors[floorNum].damage) // 플레이어가 검은 발판을 밟으면 데미지
        {
            Debug.Log("검은 발판");
            ErrorCount();
            floors[floorNum].damage = false;
        }
    }

    void TutorialStart() // 튜토리얼 게임시작
    {
        for (int i = 0; i < floorRow * floorCol; i++)
        {
            if ((i / floorCol == 0) || (i / floorCol == floorCol - 1))
            {
                floorsPosition[i].SetActive(true);
                floorsPosition[i].transform.position = new Vector3(i % floorCol, -1 * i / floorCol + 10);
                floors[i].FloorStart(i, floorCol);
            }
        }
    }

    void PlayerAppear()
    {
        playerManager.PlayerPosition(playerX, playerY);
        PauseGame(1);
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
    void PatternStart()
    {
        Debug.Log("시작");
        playerManager.PlayerStart(false);
        //patternStart = true;
        //nextPattern = true;
        time = 0;

        ButtonOn();
    }

    void ButtonOn()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].interactable = true;
        }

        if (animationHint)
        {
            for (int i = 0; i < ButtonAnimators.Length; i++)
            {
                ButtonAnimators[i].SetTrigger("isStart");
            }
        }
    }

    void ButtonOff()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].interactable = false;
        }

        for (int i = 0; i < ButtonAnimators.Length; i++)
        {
            ButtonAnimators[i].SetTrigger("isStop");
        }
    }

    void PauseGame(int i) // 대화창 표시
    {
        Time.timeScale = 0;
        talkPanel.SetActive(true);
        talkIndex = 0;

        talkID = i;
        talkText.text = talkManager.GetTalk(talkID, talkIndex++);
    }

    public void Talk() // 대화창 확인버튼
    {
        string talkData = talkManager.GetTalk(talkID, talkIndex);

        if (talkData == null) // 대화 끝
        {
            Time.timeScale = 1;
            talkPanel.SetActive(false);

            if (talkID == 1) // 튜토리얼1 시작
            {
                CountDown3();
                Invoke("Tutorial1", 3f);
            }
            else if (talkID == 2) // 튜토리얼1 종료, 바닥폭발 샘플
            {
                if (floorNum == 4)
                {
                    floorPattern(3);
                }
                else
                {
                    floorPattern(4);
                }
                Invoke("Explosion_End", 4f);
            }
            else if (talkID == 3) // 튜토리얼2 시작
            {
                TutorialStart();
                CountDown3();
                Invoke("Tutorial2", 3f);
            }
            else // 튜토리얼2 종료
            {
                SceneManager.LoadScene("Main Scene");
            }

            return;
        }

        talkText.text = talkData;
        talkIndex++;
    }

    void Tutorial1()
    {
        tutorial1 = true;
        jumpSucess.SetActive(true);
        jumpSucessText.text = "점프를 " + jumpSucessCount.ToString() + "번 하세요";
    }

    void Tutorial1_End()
    {
        ButtonOff();
        jumpSucess.SetActive(false);
        PauseGame(2);
    }

    void Explosion_End()
    {
        PauseGame(3);
    }

    void Tutorial2()
    {
        tutorial2 = true;
        nextPattern = true;
    }


    public void StageEnd()
    {
        SceneManager.LoadScene("Main Scene");
    }

    public void PauseButton() // 일시정지 버튼
    {
        Time.timeScale = 0;
        pause.SetActive(true);
    }

    public void ContinueButton() // 계속하기 버튼
    {
        Time.timeScale = 1;
        pause.SetActive(false);
    }

    public void ExitButton() // 나가기 버튼
    {
        SceneManager.LoadScene("Main Scene");
    }
}

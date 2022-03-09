using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using DG.Tweening;
using TMPro;

public class GameManager : MonoBehaviour
{
    SFXManager sfxManager;
    BGMManager bgmManager;
    public CamaerManager cameraManager;

    public StageDataManager stageData;
    public GameObject cameraPosition;
    public GameObject playerPosition;
    public GameObject[] floorsPosition;
    public FloorManager[] floors;
    public Animator[] explosion;
    public PlayerManager playerManager;

    public GameObject pause;
    public float backButtonTime; // 스마트폰 뒤로가기 버튼용 타이머
    public bool backButton; // 스마트폰 뒤로가기 버튼 클릭가능 여부
    public GameObject option;
    public AudioMixer audioMixer;
    public Slider bgmSlider;
    public Slider sfxSlider;
    public float sfx;
    public float bgm;

    public Button[] buttons;
    public Animator[] countDowns;
    public Animator[] buttonAnimations;
    public Animator speachAnimator;
    public TextMeshPro[] speachTexts; // 스테이지 정보 텍스트
    public Animator endAnimation;
    public Text endText;
    public Text endScoreText;

    public bool start; // 맵, 플레이어 등장

    public bool buttonOn; // 버튼을 누르는 타이밍 여부
    public bool buttonClick; // 버튼을 눌렀는지 여부
    public bool isPattern; // 패턴 진행 여부

    public bool playerMove; // 패턴없이 이동만 시작
    public int maxStage; // 진행가능한 가장 높은 스테이지
    public int curStage; // 방금 완료한 스테이지
    public int playerX; // 플레이어 위치
    public int playerY;

    public int floorRow; // 발판의 행
    public int floorCol; // 발판의 열
    public int floorNum; // 플레이어가 밟은 발판 번호

    public bool hint; // 버튼 힌트 효과음 여부
    public bool colorHint; // 버튼 색깔 힌트
    public float beatTime; // 버튼 효과음용 시간
    public float time; // 발판패턴용 시간
    public float patternTime; // 패턴주기
    public float patternAccuracy; // 박자 정확도
    public bool animationHint; // 박자 힌트 여부 (이동버튼)

    public Text errorText;
    public int errorCount; // 틀린 횟수
    public int cutLine; // 클리어 커트라인
    public int stageNum; // 현재 스테이지 번호

    public bool patternStart; // 패턴 시작
    public bool nextPattern; // 다음패턴 시작여부
    public int[] patternNums; // 패턴순서 모음
    public int curPatternNum; // 현재 패턴순서
    public int pattern; // 현재 패턴번호
    public int startNum; // 패턴 시작발판
    public int small; // 테두리 개수

    void Awake()
    {
        sfx = PlayerPrefs.GetFloat("SFX");
        bgm = PlayerPrefs.GetFloat("BGM");
        sfxSlider.value = sfx;
        bgmSlider.value = bgm;
        sfxManager = GameObject.Find("SFXManager").GetComponent<SFXManager>();
        bgmManager = GameObject.Find("BGMManager").GetComponent<BGMManager>();
        sfxManager.PlaySound(7);
        bgmManager.PlaySound(0);

        cameraManager.Zoom(5);
        Time.timeScale = 1;
        stageData = GameObject.Find("Stage Data").GetComponent<StageDataManager>();
        maxStage = PlayerPrefs.GetInt("Max Stage");
        errorText.text = errorCount.ToString();
        curStage = bgmManager.curStageNum;
        if (curStage < 1)
        {
            curStage = maxStage;
            if (maxStage == 50)
            {
                curStage--;
            }
        }
        playerX = ((curStage - 1) % floorRow) * 2;
        playerY = -2 * ((curStage - 1) / floorRow);
        floorNum = curStage - 1;
        cameraPosition.transform.position = new Vector3(playerX, playerY, -10);
        playerPosition.transform.position = new Vector3(playerX, playerY + 10);

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
                if(option.activeSelf) // 옵션이 켜져있으면 끄기
                {
                    OptionCloseButton();
                }
                else if (pause.activeSelf) // 일시정지창이 켜져있으면 끄기
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
        beatTime += Time.deltaTime;

        if (start)
        {
            if (time > 0.5f) // 플레이어 등장
            {
                playerManager.PlayerPosition(playerX, playerY);
                start = false;
                Invoke("PlayerMoveStart", 0.5f);
            }
        }

        if(playerMove)
        {
            if (time > patternTime && hint)
            {
                beatTime = 0;
                hint = false;
                sfxManager.PlaySound(9);
            }
            else if (beatTime > 0.5f)
            {
                beatTime = 0;
                sfxManager.PlaySound(2);
            }

            if (time > patternTime * (1 - patternAccuracy) && !buttonClick && !buttonOn)
            {
                Debug.Log("버튼 활성화");
                hint = true;
                buttonOn = true;
            }

            else if(buttonClick && time > patternTime)
            {
                buttonClick = false;
                time -= patternTime;
            }

            else if(time > patternTime * (1 + patternAccuracy))
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
                isPattern = true;

                if (colorHint)
                {
                    for (int i = 0; i < buttons.Length; i++)
                    {
                        buttons[i].image.DOColor(new Color(1, 0, 0), patternTime * patternAccuracy);
                        buttons[i].image.DOColor(new Color(1, 1, 1), patternTime * patternAccuracy).SetDelay(patternTime * patternAccuracy);
                    }
                    colorHint = false;
                }
            }
            // 정해진 시간초과
            else if (time > patternTime * (1 + patternAccuracy))
            {
                Debug.Log("시간초과");
                ErrorCount();
                buttonOn = false;
                buttonClick = false;
                colorHint = true;
                time -= patternTime;
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
                    endAnimation.SetTrigger("isDown");
                    bgmManager.StopSound();

                    if (cutLine < errorCount) // 실패
                    {
                        sfxManager.PlaySound(6);
                        endText.text = "실 패";
                        endScoreText.text = "실패 : " + "<color=#FA6464>" + errorCount.ToString() + "</color>" + " / " + cutLine.ToString();
                    }
                    else // 클리어
                    {
                        int maxStage = PlayerPrefs.GetInt("Max Stage");

                        sfxManager.PlaySound(5);
                        if (maxStage == stageNum)
                        {
                            PlayerPrefs.SetInt("Max Stage", stageNum);
                        }

                        endText.text = "성 공";
                        endScoreText.text = "실패 : " + errorCount.ToString() + " / " + cutLine.ToString();
                    }

                    for (int i = 0; i < buttons.Length; i++)
                    {
                        buttons[i].interactable = false;
                    }

                    for (int i = 0; i < buttonAnimations.Length; i++)
                    {
                        buttonAnimations[i].SetTrigger("isStop");
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

            if (time > patternTime && isPattern)
            {
                isPattern = false;
                if (buttonClick)
                {
                    time -= patternTime;
                    buttonClick = false;
                }
                switch (pattern)
                {
                    case 0: // 공백타임
                        nextPattern = true;
                        break;
                    case 1: // 처음부터 끝까지 가로 순서대로
                        floorPattern(startNum++);
                        if (startNum >= floorRow * floorCol)
                        {
                            nextPattern = true;
                        }
                        break;
                    case 2: // 처음부터 끝까지 세로 순서대로
                        floorPattern(startNum);
                        startNum += floorCol;
                        if (startNum >= floorRow * floorCol && startNum < (floorRow + 1) * floorCol - 1)
                        {
                            startNum = (startNum + 1) % floorCol;
                        }
                        if (startNum >= floorRow * floorCol)
                        {
                            nextPattern = true;
                        }
                        break;
                    case 3: // 끝부터 처음까지 가로 순서대로
                        floorPattern(startNum--);
                        if (startNum < 0)
                        {
                            nextPattern = true;
                        }
                        break;
                    case 4: // 끝부터 처음까지 세로 순서대로
                        floorPattern(startNum);
                        startNum -= floorCol;
                        if (startNum < 0 && startNum > -1 * floorCol)
                        {
                            startNum = (startNum - 1) + floorRow * floorCol;
                        }
                        if (startNum < 0)
                        {
                            nextPattern = true;
                        }
                        break;
                    case 5: // 세로줄 왼쪽 순서대로
                        for (int i = 0; i < floorRow; i++)
                        {
                            floorPattern(startNum + i * floorCol);
                        }
                        startNum += 1;
                        if (startNum > floorCol - 2)
                        {
                            nextPattern = true;
                        }
                        break;
                    case 6: // 세로줄 오른쪽 순서대로
                        for (int i = 0; i < floorRow; i++)
                        {
                            floorPattern(startNum + i * floorCol);
                        }
                        startNum -= 1;
                        if (startNum < 1)
                        {
                            nextPattern = true;
                        }
                        break;
                    case 7: // 가로줄 위 순서대로
                        for (int i = 0; i < floorCol; i++)
                        {
                            floorPattern(startNum + i);
                        }
                        startNum += floorCol;
                        if (startNum / floorCol >= floorRow - 1)
                        {
                            nextPattern = true;
                        }
                        break;
                    case 8: // 가로줄 아래 순서대로
                        for (int i = 0; i < floorCol; i++)
                        {
                            floorPattern(startNum + i);
                        }
                        startNum -= floorCol;
                        if (startNum < 1)
                        {
                            nextPattern = true;
                        }
                        break;
                    case 9: // 가장자리에서 순서대로
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
                        if (startNum > small)
                        {
                            nextPattern = true;
                        }
                        break;
                    case 10:  // 가운데에서 순서대로
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
                        if (startNum < 0)
                        {
                            nextPattern = true;
                        }
                        break;
                    case 11: // 홀짝
                        for (int i = 0; i < floorRow * floorCol; i++)
                        {
                            if (i % 2 == startNum % 2)
                            {
                                floorPattern(i);
                            }
                        }
                        startNum++;
                        if (startNum > 10)
                        {
                            nextPattern = true;
                        }
                        break;
                }
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
            sfxManager.PlaySound(1);
            buttonOn = false;
            buttonClick = true;
            SpeachBubbleOff();
            switch (n)
            {
                case 0:
                    if (floorNum - floorCol < 0 || floorNum == floorRow * floorCol)
                    {
                        Debug.Log("방향 틀림");
                        playerPosition.transform.DOMoveY(playerY + 0.4f, 0.25f / (2 / patternTime)).SetEase(Ease.OutCubic);
                        playerPosition.transform.DOScale(new Vector3(0.7f, 0.7f, 0), 0.25f / (2 / patternTime)).SetEase(Ease.OutCubic);
                        playerPosition.transform.DOMoveY(playerY, 0.25f / (2 / patternTime)).SetEase(Ease.InCubic).SetDelay(0.25f / (2 / patternTime));
                        playerPosition.transform.DOScale(new Vector3(0.5f, 0.5f, 0), 0.25f / (2 / patternTime)).SetEase(Ease.InCubic).SetDelay(0.25f / (2 / patternTime));
                        ErrorCount();
                        Invoke("SpeachBubbleOn", 0.5f / (2 / patternTime));
                    }
                    else
                    {
                        playerY += 2;
                        floorNum -= floorCol;
                        playerPosition.transform.DOMoveY(playerY - 0.5f, 0.5f / (2 / patternTime)).SetEase(Ease.OutCubic);
                        playerPosition.transform.DOScale(new Vector3(0.8f, 0.8f, 0), 0.5f / (2 / patternTime)).SetEase(Ease.OutCubic);
                        playerPosition.transform.DOMoveY(playerY, 0.5f / (2 / patternTime)).SetEase(Ease.InCubic).SetDelay(0.5f / (2 / patternTime));
                        playerPosition.transform.DOScale(new Vector3(0.5f, 0.5f, 0), 0.5f / (2 / patternTime)).SetEase(Ease.InCubic).SetDelay(0.5f / (2 / patternTime));
                        Invoke("SpeachBubbleOn", 1f / (2 / patternTime));
                    }
                    break;
                case 1:
                    if (floorNum + floorCol >= floorRow * floorCol || floorNum == floorRow * floorCol)
                    {
                        Debug.Log("방향 틀림");
                        playerPosition.transform.DOMoveY(playerY - 0.4f, 0.25f / (2 / patternTime)).SetEase(Ease.OutCubic);
                        playerPosition.transform.DOScale(new Vector3(0.7f, 0.7f, 0), 0.25f / (2 / patternTime)).SetEase(Ease.OutCubic);
                        playerPosition.transform.DOMoveY(playerY, 0.25f / (2 / patternTime)).SetEase(Ease.InCubic).SetDelay(0.25f / (2 / patternTime));
                        playerPosition.transform.DOScale(new Vector3(0.5f, 0.5f, 0), 0.25f / (2 / patternTime)).SetEase(Ease.InCubic).SetDelay(0.25f / (2 / patternTime));
                        ErrorCount();
                        Invoke("SpeachBubbleOn", 0.5f / (2 / patternTime));
                    }
                    else
                    {
                        playerY -= 2;
                        floorNum += floorCol;
                        playerPosition.transform.DOMoveY(playerY + 1f, 0.5f / (2 / patternTime)).SetEase(Ease.OutCubic);
                        playerPosition.transform.DOScale(new Vector3(0.8f, 0.8f, 0), 0.5f / (2 / patternTime)).SetEase(Ease.OutCubic);
                        playerPosition.transform.DOMoveY(playerY, 0.5f / (2 / patternTime)).SetEase(Ease.InCubic).SetDelay(0.5f / (2 / patternTime));
                        playerPosition.transform.DOScale(new Vector3(0.5f, 0.5f, 0), 0.5f / (2 / patternTime)).SetEase(Ease.InCubic).SetDelay(0.5f / (2 / patternTime));
                        Invoke("SpeachBubbleOn", 1f / (2 / patternTime));
                    }
                    break;
                case 2:
                    if (floorNum % floorCol == 0)
                    {
                        if (floorNum == floorRow * floorCol)
                        {
                            playerX -= 2;
                            floorNum -= 1;
                            playerPosition.transform.DOMoveX(playerX, 1f / (2 / patternTime)).SetEase(Ease.Linear);
                            playerPosition.transform.DOMoveY(playerY + 1f, 0.5f / (2 / patternTime)).SetEase(Ease.OutQuart);
                            playerPosition.transform.DOMoveY(playerY, 0.5f / (2 / patternTime)).SetEase(Ease.InQuart).SetDelay(0.5f / (2 / patternTime));
                            Invoke("SpeachBubbleOn", 1f / (2 / patternTime));
                        }
                        else
                        {
                            Debug.Log("방향 틀림");
                            playerPosition.transform.DOMoveX(playerX - 0.25f, 0.25f / (2 / patternTime)).SetEase(Ease.Linear);
                            playerPosition.transform.DOMoveY(playerY + 0.25f, 0.25f / (2 / patternTime)).SetEase(Ease.OutCubic);
                            playerPosition.transform.DOMoveX(playerX, 0.25f / (2 / patternTime)).SetEase(Ease.Linear).SetDelay(0.25f / (2 / patternTime));
                            playerPosition.transform.DOMoveY(playerY, 0.25f / (2 / patternTime)).SetEase(Ease.InCubic).SetDelay(0.25f / (2 / patternTime));
                            ErrorCount();
                            Invoke("SpeachBubbleOn", 0.5f / (2 / patternTime));
                        }
                    }
                    else
                    {
                        playerX -= 2;
                        floorNum -= 1;
                        playerPosition.transform.DOMoveX(playerX, 1f / (2 / patternTime)).SetEase(Ease.Linear);
                        playerPosition.transform.DOMoveY(playerY + 1f, 0.5f / (2 / patternTime)).SetEase(Ease.OutQuart);
                        playerPosition.transform.DOMoveY(playerY, 0.5f / (2 / patternTime)).SetEase(Ease.InQuart).SetDelay(0.5f / (2 / patternTime));
                        Invoke("SpeachBubbleOn", 1f / (2 / patternTime));
                    }
                    break;
                case 3:
                    if (floorNum % floorCol == floorCol - 1)
                    {
                        if (floorNum == floorRow * floorCol - 1)
                        {
                            playerX += 2;
                            floorNum += 1;
                            playerPosition.transform.DOMoveX(playerX, 1f / (2 / patternTime)).SetEase(Ease.Linear);
                            playerPosition.transform.DOMoveY(playerY + 1f, 0.5f / (2 / patternTime)).SetEase(Ease.OutQuart);
                            playerPosition.transform.DOMoveY(playerY, 0.5f / (2 / patternTime)).SetEase(Ease.InQuart).SetDelay(0.5f / (2 / patternTime));
                            Invoke("SpeachBubbleOn", 1f / (2 / patternTime));
                        }
                        else
                        {
                            Debug.Log("방향 틀림");
                            playerPosition.transform.DOMoveX(playerX + 0.25f, 0.25f / (2 / patternTime)).SetEase(Ease.Linear);
                            playerPosition.transform.DOMoveY(playerY + 0.25f, 0.25f / (2 / patternTime)).SetEase(Ease.OutCubic);
                            playerPosition.transform.DOMoveX(playerX, 0.25f / (2 / patternTime)).SetEase(Ease.Linear).SetDelay(0.25f / (2 / patternTime));
                            playerPosition.transform.DOMoveY(playerY, 0.25f / (2 / patternTime)).SetEase(Ease.InCubic).SetDelay(0.25f / (2 / patternTime));
                            ErrorCount();
                            Invoke("SpeachBubbleOn", 0.5f / (2 / patternTime));
                        }
                    }
                    else
                    {
                        if (floorNum == floorRow * floorCol)
                        {
                            Debug.Log("방향 틀림");
                            playerPosition.transform.DOMoveX(playerX + 0.25f, 0.25f / (2 / patternTime)).SetEase(Ease.Linear);
                            playerPosition.transform.DOMoveY(playerY + 0.25f, 0.25f / (2 / patternTime)).SetEase(Ease.OutCubic);
                            playerPosition.transform.DOMoveX(playerX, 0.25f / (2 / patternTime)).SetEase(Ease.Linear).SetDelay(0.25f / (2 / patternTime));
                            playerPosition.transform.DOMoveY(playerY, 0.25f / (2 / patternTime)).SetEase(Ease.InCubic).SetDelay(0.25f / (2 / patternTime));
                            ErrorCount();
                            Invoke("SpeachBubbleOn", 0.5f / (2 / patternTime));
                        }
                        else
                        {
                            playerX += 2;
                            floorNum += 1;
                            playerPosition.transform.DOMoveX(playerX, 1f / (2 / patternTime)).SetEase(Ease.Linear);
                            playerPosition.transform.DOMoveY(playerY + 1f, 0.5f / (2 / patternTime)).SetEase(Ease.OutQuart);
                            playerPosition.transform.DOMoveY(playerY, 0.5f / (2 / patternTime)).SetEase(Ease.InQuart).SetDelay(0.5f / (2 / patternTime));
                            Invoke("SpeachBubbleOn", 1f / (2 / patternTime));
                        }
                    }
                    break;
            }
            cameraPosition.transform.DOMove(new Vector3(playerX, playerY, -10), 1f);
            Invoke("FloorDamage", 1f);
        }
        else // 버튼을 누르는 타이밍이 아님
        {
            Debug.Log("타이밍 틀림");
            ErrorCount();
        }
    }

    void SpeachBubbleOn() // 스테이지 정보 말풍선 켜기
    {
        speachAnimator.SetTrigger("isSpeachOn");
        speachTexts[0].text = (floorNum + 1).ToString();
        speachTexts[1].text = stageData.stageDatas[floorNum + 1].floorRow.ToString() + "×" + stageData.stageDatas[floorNum + 1].floorCol.ToString();
        speachTexts[2].text = stageData.stageDatas[floorNum + 1].cutLine.ToString() + "회 이하";
        speachTexts[3].text = stageData.stageDatas[floorNum + 1].patternTime.ToString() + "초";
        speachTexts[4].text = (stageData.stageDatas[floorNum + 1].patternAccuracy * 100).ToString() + " %";
    }

    void SpeachBubbleOff() // 스테이지 정보 말풍선 끄기
    {
        speachAnimator.SetTrigger("isSpeachOff");
    }

    void FloorDamage()
    {
        if (floors[floorNum].damage) // 플레이어가 검은 발판을 밟으면 데미지
        {
            Debug.Log("검은 발판");
            ErrorCount();
            floors[floorNum].damage = false;
            explosion[floorNum].SetTrigger("isExplosion");
            sfxManager.PlaySound(3);
        }
    }

    public void StartButton()
    {
        if (floorNum < maxStage)
        {
            playerMove = false;
            if (floorNum == 49) // 마지막 맵
            {
                for (int i = 0; i < buttons.Length; i++)
                {
                    buttons[i].interactable = false;
                }
                for (int i = 0; i < buttonAnimations.Length; i++)
                {
                    buttonAnimations[i].SetTrigger("isStop");
                }

                playerPosition.transform.DOMoveY(floorsPosition[24].transform.position.y, 1f);
                floorsPosition[49].transform.DOMoveY(floorsPosition[24].transform.position.y, 1f);
                cameraPosition.transform.DOMoveY(floorsPosition[24].transform.position.y, 1f);
                playerPosition.transform.DOMoveX(floorsPosition[24].transform.position.x, 1f).SetDelay(1f);
                floorsPosition[49].transform.DOMoveX(floorsPosition[24].transform.position.x, 1f).SetDelay(1f);
                cameraPosition.transform.DOMoveX(floorsPosition[24].transform.position.x, 1f).SetDelay(1f);
                SpeachBubbleOff();
                Invoke("FinalStage", 3f);
            }
            else
            {
                sfxManager.PlaySound(8);
                cameraManager.Zoom(0);
                bgmManager.StopSound();
                stageData.stageDatas[0] = stageData.stageDatas[floorNum + 1];
                bgmManager.curStageNum = floorNum + 1;
                LoadingCanvasManager.Instance.ChangeScene("Stage Scene");
            }
        }
        else
        {
            // 경고음
            sfxManager.PlaySound(3);
        }
    }

    void FinalStage() // 마지막 스테이지 시작
    {
        bgmManager.StopSound();
        cameraManager.Zoom(8);
        errorCount = 0;
        errorText.text = errorCount.ToString();
        stageNum = stageData.stageDatas[50].stageNum;
        cutLine = stageData.stageDatas[50].cutLine;
        floorRow = stageData.stageDatas[50].floorRow;
        floorCol = stageData.stageDatas[50].floorCol;
        patternTime = stageData.stageDatas[50].patternTime;
        patternAccuracy = stageData.stageDatas[50].patternAccuracy;
        patternNums = stageData.stageDatas[50].patternNums;
        for (int i = 0; i < floorRow * floorCol; i++)
        {
            floors[i].PatternTime();
        }
        playerX = 6;
        playerY = -6;
        floorNum = 24;
        CountDown3();
    }

    public void StageEnd() // 스테이지 종료 버튼
    {
        sfxManager.PlaySound(8);
        endAnimation.SetTrigger("isUp");
        if (cutLine < errorCount) // 실패
        {
            bgmManager.PlaySound(0);
            cameraManager.Zoom(5);
            patternTime = 2f;
            patternAccuracy = 0.2f;
            playerX = 14;
            playerY = -12;
            floorNum = 49;

            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].interactable = true;
            }
            for (int i = 0; i < buttonAnimations.Length; i++)
            {
                buttonAnimations[i].SetTrigger("isStart");
            }

            playerPosition.transform.DOMoveX(playerX, 1f);
            floorsPosition[49].transform.DOMoveX(playerX, 1f);
            cameraPosition.transform.DOMoveX(playerX, 1f);
            playerPosition.transform.DOMoveY(playerY, 1f).SetDelay(1f);
            floorsPosition[49].transform.DOMoveY(playerY, 1f).SetDelay(1f);
            cameraPosition.transform.DOMoveY(playerY, 1f).SetDelay(1f);
            Invoke("PlayerMoveStart", 2f);
        }
        else // 성공
        {
            playerPosition.transform.DOMoveY(10, 3f).SetEase(Ease.InQuart);
            floorsPosition[49].transform.DOMoveY(10, 3f).SetEase(Ease.InQuart);
            cameraPosition.transform.DOMoveY(10, 3f).SetEase(Ease.InQuart);
            Invoke("Ending", 3.5f);
        }
    }

    void Ending()
    {
        LoadingCanvasManager.Instance.ChangeScene("Ending Scene");
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
        sfxManager.PlaySound(11);
        countDowns[2].SetTrigger("isStart");
        Invoke("CountDown2", 1f);
    }
    void CountDown2()
    {
        sfxManager.PlaySound(11);
        countDowns[1].SetTrigger("isStart");
        Invoke("CountDown1", 1f);
    }
    void CountDown1()
    {
        sfxManager.PlaySound(11);
        countDowns[0].SetTrigger("isStart");
        Invoke("PatternStart", 1f);
    }
    void PatternStart() // 패턴시작
    {
        Debug.Log("시작");
        sfxManager.PlaySound(11);
        bgmManager.PlaySound(50);
        playerManager.PlayerStart(false);
        playerMove = false;
        patternStart = true;
        nextPattern = true;
        time = 0;

        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].interactable = true;
        }
    }

    void PlayerMoveStart() // 패턴없이 박자에 맞춰서 이동만
    {
        playerManager.PlayerStart(false);
        playerMove = true;
        patternStart = false;
        nextPattern = false;
        hint = true;
        time = 0;
        beatTime = 0;

        SpeachBubbleOn();

        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].interactable = true;
        }
        for (int i = 0; i < buttonAnimations.Length; i++)
        {
            buttonAnimations[i].SetTrigger("isStart");
        }
    }

    public void OptionButton() // 옵션 버튼
    {
        sfxManager.PlaySound(0);
        Time.timeScale = 0;
        option.SetActive(true);
    }

    public void BGMAudioControl() // 볼륨조절
    {
        float bgm = bgmSlider.value;

        if (bgm == -40)
        {
            audioMixer.SetFloat("BGM", -80);
        }
        else
        {
            audioMixer.SetFloat("BGM", bgm);
        }

        PlayerPrefs.SetFloat("BGM", bgm);
    }

    public void SFXAudioControl() // 볼륨조절
    {
        float sfx = sfxSlider.value;

        if (sfx == -40)
        {
            audioMixer.SetFloat("SFX", -80);
        }
        else
        {
            audioMixer.SetFloat("SFX", sfx);
        }

        PlayerPrefs.SetFloat("SFX", sfx);
    }

    public void OptionCloseButton() // 옵션 닫기
    {
        sfxManager.PlaySound(0);
        Time.timeScale = 1;
        option.SetActive(false);
    }

    public void PauseButton() // 일시정지 버튼
    {
        sfxManager.PlaySound(0);
        Time.timeScale = 0;
        pause.SetActive(true);
    }

    public void ContinueButton() // 계속하기 버튼
    {
        sfxManager.PlaySound(0);
        Time.timeScale = 1;
        pause.SetActive(false);
    }

    public void ExitButton() // 나가기 버튼
    {
        playerMove = false;
        sfxManager.PlaySound(0);
        Application.Quit();
    }
}

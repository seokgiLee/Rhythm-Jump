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
    public float backButtonTime; // ����Ʈ�� �ڷΰ��� ��ư�� Ÿ�̸�
    public bool backButton; // ����Ʈ�� �ڷΰ��� ��ư Ŭ������ ����
    public GameObject option;
    public AudioMixer audioMixer;
    public Slider bgmSlider;
    public Slider sfxSlider;

    public Button[] buttons;
    public Animator[] countDowns;
    public Animator[] buttonAnimations;
    public Animator speachAnimator;
    public TextMeshPro[] speachTexts; // �������� ���� �ؽ�Ʈ
    public Animator endAnimation;
    public Text endText;
    public Text endScoreText;

    public bool start; // ��, �÷��̾� ����

    public bool buttonOn; // ��ư�� ������ Ÿ�̹� ����
    public bool buttonClick; // ��ư�� �������� ����

    public bool playerMove; // ���Ͼ��� �̵��� ����
    public int maxStage; // ���డ���� ���� ���� ��������
    public int curStage; // ��� �Ϸ��� ��������
    public int playerX; // �÷��̾� ��ġ
    public int playerY;

    public int floorRow; // ������ ��
    public int floorCol; // ������ ��
    public int floorNum; // �÷��̾ ���� ���� ��ȣ

    public float beatTime; // ��ư ȿ������ �ð�
    public float time; // �������Ͽ� �ð�
    public float patternTime; // �����ֱ�
    public float patternAccuracy; // ���� ��Ȯ��
    public bool animationHint; // ���� ��Ʈ ���� (�̵���ư)

    public Text errorText;
    public int errorCount; // Ʋ�� Ƚ��
    public int cutLine; // Ŭ���� ĿƮ����
    public int stageNum; // ���� �������� ��ȣ

    public bool patternStart; // ���� ����
    public bool nextPattern; // �������� ���ۿ���
    public int[] patternNums; // ���ϼ��� ����
    public int curPatternNum; // ���� ���ϼ���
    public int pattern; // ���� ���Ϲ�ȣ
    public int startNum; // ���� ���۹���
    public int small; // �׵θ� ����

    void Awake()
    {
        sfxManager = GameObject.Find("SFXManager").GetComponent<SFXManager>();
        bgmManager = GameObject.Find("BGMManager").GetComponent<BGMManager>();
        sfxManager.PlaySound(7);

        cameraManager.Zoom(5);
        Time.timeScale = 1;
        stageData = GameObject.Find("Stage Data").GetComponent<StageDataManager>();
        maxStage = PlayerPrefs.GetInt("Max Stage");
        errorText.text = errorCount.ToString();
        if (curStage < 1)
        {
            curStage = maxStage;
        }
        playerX = (curStage - 1) * 2 % floorRow;
        playerY = -2 * ((curStage - 1) / floorRow);
        floorNum = playerX / 2 + -1 * playerY / 2 * floorCol;
        cameraPosition.transform.position = new Vector3(playerX, playerY, -10);
        playerPosition.transform.position = new Vector3(playerX, playerY + 10);

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
        if (!backButton)
        {
            backButtonTime += Time.deltaTime;
            if (backButtonTime > 0.5f) // 0.5�ʸ��� Ŭ������
            {
                backButton = true;
                time = 0;
            }
        }

        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Escape) && backButton) // �ڷΰ���
            {
                if(option.activeSelf) // �ɼ��� ���������� ����
                {
                    OptionCloseButton();
                }
                else if (pause.activeSelf) // �Ͻ�����â�� ���������� ����
                {
                    ContinueButton();
                }
                else // �Ͻ�����â�� ���������� �Ͻ�����
                {
                    PauseButton();
                }
                backButton = false;
            }
        }

        time += Time.deltaTime;

        if (start)
        {
            if (time > 0.5f) // �÷��̾� ����
            {
                playerManager.PlayerPosition(playerX, playerY);
                start = false;
                //Invoke("CountDown3", 0.5f);
                Invoke("PlayerMoveStart", 0.5f);
            }
        }

        if(playerMove)
        {
            beatTime += Time.deltaTime;

            if (beatTime > 0.5f)
            {
                beatTime = 0;
                sfxManager.PlaySound(2);
            }

            if (time > patternTime * (1 - patternAccuracy) && !buttonClick && !buttonOn)
            {
                Debug.Log("��ư Ȱ��ȭ");
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
            // ������ �ð��� ����
            if (time > patternTime * (1 - patternAccuracy) && !buttonClick && !buttonOn)
            {
                // ��ư Ȱ��ȭ
                Debug.Log("��ư Ȱ��ȭ");
                buttonOn = true;
            }
            // ������ �ð��ʰ�
            else if (time > patternTime * patternAccuracy && time < patternTime * (1 - patternAccuracy) && buttonOn)
            {
                Debug.Log("�ð��ʰ�");
                ErrorCount();
                buttonOn = false;

                FloorDamage();
            }

            if (nextPattern)
            {
                nextPattern = false;
                if (curPatternNum >= patternNums.Length)
                {
                    // �� ����
                    pattern = 0;
                    patternStart = false;
                    endAnimation.SetTrigger("isDown");

                    if (cutLine < errorCount) // ����
                    {
                        sfxManager.PlaySound(6);
                        endText.text = "�� ��";
                        endScoreText.text = "���� : " + "<color=#FA6464>" + errorCount.ToString() + "</color>" + " / " + cutLine.ToString();
                    }
                    else // Ŭ����
                    {
                        int maxStage = PlayerPrefs.GetInt("Max Stage");

                        sfxManager.PlaySound(5);
                        if (maxStage == stageNum)
                        {
                            PlayerPrefs.SetInt("Max Stage", stageNum);
                        }

                        endText.text = "�� ��";
                        endScoreText.text = "���� : " + errorCount.ToString() + " / " + cutLine.ToString();
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
                case 0: // ����Ÿ��
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
                case 1: // ó������ ������ ���� �������
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
                case 2: // ó������ ������ ���� �������
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
                case 3: // ������ ó������ ���� �������
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
                case 4: // ������ ó������ ���� �������
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
                case 5: // ������ ���� �������
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
                case 6: // ������ ������ �������
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
                case 7: // ������ �� �������
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
                case 8: // ������ �Ʒ� �������
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
                case 9: // �����ڸ����� �������
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
                case 10:  // ������� �������
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
                case 11: // Ȧ¦
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

    public void floorPattern(int n) // ���� ���� ����
    {
        floors[n].floorPatternStart();
    }

    public void MoveButton(int n)
    {
        if (buttonOn) // ��ư�� ������ Ÿ�̹�
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
                        Debug.Log("���� Ʋ��");
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
                        Debug.Log("���� Ʋ��");
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
                            Debug.Log("���� Ʋ��");
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
                            Debug.Log("���� Ʋ��");
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
                            Debug.Log("���� Ʋ��");
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
        else // ��ư�� ������ Ÿ�̹��� �ƴ�
        {
            Debug.Log("Ÿ�̹� Ʋ��");
            ErrorCount();
        }
    }

    void SpeachBubbleOn() // �������� ���� ��ǳ�� �ѱ�
    {
        speachAnimator.SetTrigger("isSpeachOn");
        speachTexts[0].text = (floorNum + 1).ToString();
        speachTexts[1].text = stageData.stageDatas[floorNum + 1].floorRow.ToString() + "��" + stageData.stageDatas[floorNum + 1].floorCol.ToString();
        speachTexts[2].text = stageData.stageDatas[floorNum + 1].cutLine.ToString() + "ȸ ����";
        speachTexts[3].text = stageData.stageDatas[floorNum + 1].patternTime.ToString() + "��";
        speachTexts[4].text = (stageData.stageDatas[floorNum + 1].patternAccuracy * 100).ToString() + " %";
    }

    void SpeachBubbleOff() // �������� ���� ��ǳ�� ����
    {
        speachAnimator.SetTrigger("isSpeachOff");

    }

    void FloorDamage()
    {
        if (floors[floorNum].damage) // �÷��̾ ���� ������ ������ ������
        {
            Debug.Log("���� ����");
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
            sfxManager.PlaySound(8);
            cameraManager.Zoom(0);
            stageData.stageDatas[0] = stageData.stageDatas[floorNum + 1];
            LoadingCanvasManager.Instance.ChangeScene("Stage Scene");
        }
        else
        {
            // �����
            sfxManager.PlaySound(3);
        }
    }

    void MapStart()
    {
        start = true;
        time = 0;
    }

    public void ErrorCount() // Ʋ�� Ƚ��
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
    void PatternStart() // ���Ͻ���
    {
        Debug.Log("����");
        playerManager.PlayerStart(false);
        patternStart = true;
        nextPattern = true;
        time = 0;

        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].interactable = true;
        }
    }

    void PlayerMoveStart() // ���Ͼ��� ���ڿ� ���缭 �̵���
    {
        playerManager.PlayerStart(false);
        playerMove = true;
        time = 0;

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

    public void OptionButton() // �ɼ� ��ư
    {
        sfxManager.PlaySound(0);
        Time.timeScale = 0;
        option.SetActive(true);
    }

    public void BGMAudioControl() // ��������
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

    public void SFXAudioControl() // ��������
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

    public void OptionCloseButton() // �ɼ� �ݱ�
    {
        sfxManager.PlaySound(0);
        Time.timeScale = 1;
        option.SetActive(false);
    }

    public void PauseButton() // �Ͻ����� ��ư
    {
        sfxManager.PlaySound(0);
        Time.timeScale = 0;
        pause.SetActive(true);
    }

    public void ContinueButton() // ����ϱ� ��ư
    {
        sfxManager.PlaySound(0);
        Time.timeScale = 1;
        pause.SetActive(false);
    }

    public void ExitButton() // ������ ��ư
    {
        sfxManager.PlaySound(0);
        Application.Quit();
    }
}

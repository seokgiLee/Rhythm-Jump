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
    public float sfx;
    public float bgm;

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
    public bool isPattern; // ���� ���� ����

    public bool playerMove; // ���Ͼ��� �̵��� ����
    public int maxStage; // ���డ���� ���� ���� ��������
    public int curStage; // ��� �Ϸ��� ��������
    public int playerX; // �÷��̾� ��ġ
    public int playerY;

    public int floorRow; // ������ ��
    public int floorCol; // ������ ��
    public int floorNum; // �÷��̾ ���� ���� ��ȣ

    public bool hint; // ��ư ��Ʈ ȿ���� ����
    public bool colorHint; // ��ư ���� ��Ʈ
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
        beatTime += Time.deltaTime;

        if (start)
        {
            if (time > 0.5f) // �÷��̾� ����
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
                Debug.Log("��ư Ȱ��ȭ");
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
            // ������ �ð��� ����
            if (time > patternTime * (1 - patternAccuracy) && !buttonClick && !buttonOn)
            {
                // ��ư Ȱ��ȭ
                Debug.Log("��ư Ȱ��ȭ");
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
            // ������ �ð��ʰ�
            else if (time > patternTime * (1 + patternAccuracy))
            {
                Debug.Log("�ð��ʰ�");
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
                    // �� ����
                    pattern = 0;
                    patternStart = false;
                    endAnimation.SetTrigger("isDown");
                    bgmManager.StopSound();

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
                    case 0: // ����Ÿ��
                        nextPattern = true;
                        break;
                    case 1: // ó������ ������ ���� �������
                        floorPattern(startNum++);
                        if (startNum >= floorRow * floorCol)
                        {
                            nextPattern = true;
                        }
                        break;
                    case 2: // ó������ ������ ���� �������
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
                    case 3: // ������ ó������ ���� �������
                        floorPattern(startNum--);
                        if (startNum < 0)
                        {
                            nextPattern = true;
                        }
                        break;
                    case 4: // ������ ó������ ���� �������
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
                    case 5: // ������ ���� �������
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
                    case 6: // ������ ������ �������
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
                    case 7: // ������ �� �������
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
                    case 8: // ������ �Ʒ� �������
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
                    case 9: // �����ڸ����� �������
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
                    case 10:  // ������� �������
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
                    case 11: // Ȧ¦
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
            playerMove = false;
            if (floorNum == 49) // ������ ��
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
            // �����
            sfxManager.PlaySound(3);
        }
    }

    void FinalStage() // ������ �������� ����
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

    public void StageEnd() // �������� ���� ��ư
    {
        sfxManager.PlaySound(8);
        endAnimation.SetTrigger("isUp");
        if (cutLine < errorCount) // ����
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
        else // ����
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
    void PatternStart() // ���Ͻ���
    {
        Debug.Log("����");
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

    void PlayerMoveStart() // ���Ͼ��� ���ڿ� ���缭 �̵���
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
        playerMove = false;
        sfxManager.PlaySound(0);
        Application.Quit();
    }
}

using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    SFXManager sfxManager;
    BGMManager bgmManager;
    public CamaerManager cameraManager;

    public GameObject cameraPosition;
    public GameObject playerPosition;
    public GameObject[] floorsPosition;
    public FloorManager[] floors;
    public Animator[] explosion;
    public PlayerManager playerManager;

    public GameObject pause;
    public float backButtonTime; // ����Ʈ�� �ڷΰ��� ��ư�� Ÿ�̸�
    public bool backButton; // ����Ʈ�� �ڷΰ��� ��ư Ŭ������ ����

    public Button[] buttons;
    public Animator[] countDowns;

    public Animator[] buttonAnimations;
    public Animator moveButtonsAnimator;
    public Animator pauseButtonAnimator;

    public bool start; // ��, �÷��̾� ����

    public bool buttonOn; // ��ư�� ������ Ÿ�̹� ����
    public bool buttonClick; // ��ư�� �������� ����
    public bool isPattern; // ���� ���� ����

    public int playerX; // �÷��̾� ��ġ
    public int playerY;

    public int floorRow; // ������ ��
    public int floorCol; // ������ ��
    public int floorNum; // �÷��̾ ���� ���� ��ȣ

    public bool hint; // ��ư ��Ʈ ȿ���� ����
    public bool colorHint; // ��ư ���� ��Ʈ
    public float beatTime; // ��ư ȿ������ �ð�
    public float time; // �������Ͽ� �ð�
    public float playerTime; // �÷��̾� Ÿ�ֿ̹� �ð�
    public float patternTime; // �����ֱ�
    public float patternAccuracy; // ���� ��Ȯ��
    public bool animationHint; // ���� ��Ʈ ���� (�̵���ư)

    public Text errorText;
    public int errorCount; // Ʋ�� Ƚ��

    public bool patternStart; // ���� ����
    public bool nextPattern; // �������� ���ۿ���
    public int[] patternNums; // ���ϼ��� ����
    public int curPatternNum; // ���� ���ϼ���
    public int pattern; // ���� ���Ϲ�ȣ
    public int startNum; // ���� ���۹���
    public int isOdd; // Ȧ¦ �Ǻ�
    public int small; // �׵θ� ����

    public TalkManager talkManager;
    public GameObject talkPanel;
    public Text talkText;
    public int talkID;
    public int talkIndex;

    public GameObject jumpSucess;
    public Text jumpSucessText;
    bool tutorial1; // ù��° Ʃ�丮�� ����
    public int jumpSucessCount; // ���� ���� ����Ƚ��
    bool tutorial2; // �ι�° Ʃ�丮�� ����

    void Awake()
    {
        sfxManager = GameObject.Find("SFXManager").GetComponent<SFXManager>();
        bgmManager = GameObject.Find("BGMManager").GetComponent<BGMManager>();
        sfxManager.PlaySound(7);

        cameraManager.Zoom(5);
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
        // ����Ʈ�� �ڷΰ����ư
        if (!backButton)
        {
            backButtonTime += Time.deltaTime;
            if (backButtonTime > 0.5f) // 0.5�ʸ��� Ŭ������
            {
                backButton = true;
                backButtonTime = 0;
            }
        }

        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Escape) && backButton) // �ڷΰ���
            {
                if (pause.activeSelf) // �Ͻ�����â�� ���������� ����ϱ�
                {
                    ContinueButton();
                }
                else // �Ͻ�����â�� ���������� �Ͻ�����
                {
                    PauseButton();
                }
                backButton = false;
                StartCoroutine(BackButton());
            }
        }

        if (tutorial1) // Ʃ�丮��1 ����
        {
            time += Time.deltaTime;
            beatTime += Time.deltaTime;

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

            if (jumpSucessCount < 1) // Ʃ�丮��1 ����
            {
                buttonClick = false;
                buttonOn = false;
                tutorial1 = false;
                Invoke("Tutorial1_End", 1f / (2 / patternTime));
            }
        }

        if (tutorial2)
        {
            time += Time.deltaTime;
            beatTime += Time.deltaTime;
            playerTime += Time.deltaTime;

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

            if (buttonClick)
            {
                playerTime -= patternTime;
                buttonClick = false;
            }

            // ������ �ð��� ����
            if (playerTime > patternTime * (1 - patternAccuracy) && !buttonOn && !buttonClick)
            {
                // ��ư Ȱ��ȭ
                Debug.Log("��ư Ȱ��ȭ");
                hint = true;
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
            else if (playerTime > patternTime * (1 + patternAccuracy))
            {
                Debug.Log("�ð��ʰ�");
                ErrorCount();
                buttonOn = false;
                buttonClick = false;
                colorHint = true;
                playerTime -= patternTime;
                FloorDamage();
            }

            if (nextPattern)
            {
                nextPattern = false;
                if (curPatternNum >= patternNums.Length)
                {
                    // �� ����
                    pattern = 0;
                    tutorial2 = false;
                    patternStart = false;

                    ; PlayerPrefs.SetInt("Max Stage", 1);
                    ButtonOff();

                    sfxManager.PlaySound(5);
                    bgmManager.StopSound();
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

            if (time > patternTime && isPattern)
            {
                isPattern = false;
                time -= patternTime;
                
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
                            if (i % 2 == isOdd % 2)
                            {
                                floorPattern(i);
                            }
                        }
                        isOdd = (isOdd + 1) % 2;
                        nextPattern = true;
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
            switch (n)
            {
                case 0:
                    if (floorNum - floorCol < 0)
                    {
                        Debug.Log("���� Ʋ��");
                        playerPosition.transform.DOMoveY(playerY + 0.4f, 0.25f / (2 / patternTime)).SetEase(Ease.OutCubic);
                        playerPosition.transform.DOScale(new Vector3(0.7f, 0.7f, 0), 0.25f / (2 / patternTime)).SetEase(Ease.OutCubic);
                        playerPosition.transform.DOMoveY(playerY, 0.25f / (2 / patternTime)).SetEase(Ease.InCubic).SetDelay(0.25f / (2 / patternTime));
                        playerPosition.transform.DOScale(new Vector3(0.5f, 0.5f, 0), 0.25f / (2 / patternTime)).SetEase(Ease.InCubic).SetDelay(0.25f / (2 / patternTime));
                        ErrorCount();
                    }
                    else
                    {
                        playerY += 1;
                        floorNum -= floorCol;
                        playerPosition.transform.DOMoveY(playerY - 0.5f, 0.5f / (2 / patternTime)).SetEase(Ease.OutCubic);
                        playerPosition.transform.DOScale(new Vector3(0.8f, 0.8f, 0), 0.5f / (2 / patternTime)).SetEase(Ease.OutCubic);
                        playerPosition.transform.DOMoveY(playerY, 0.5f / (2 / patternTime)).SetEase(Ease.InCubic).SetDelay(0.5f / (2 / patternTime));
                        playerPosition.transform.DOScale(new Vector3(0.5f, 0.5f, 0), 0.5f / (2 / patternTime)).SetEase(Ease.InCubic).SetDelay(0.5f / (2 / patternTime));
                        jumpSucessCount--;
                        jumpSucessText.text = "������ " + jumpSucessCount.ToString() + "�� �ϼ���";
                    }
                    break;
                case 1:
                    if (floorNum + floorCol >= floorRow * floorCol)
                    {
                        Debug.Log("���� Ʋ��");
                        playerPosition.transform.DOMoveY(playerY - 0.4f, 0.25f / (2 / patternTime)).SetEase(Ease.OutCubic);
                        playerPosition.transform.DOScale(new Vector3(0.7f, 0.7f, 0), 0.25f / (2 / patternTime)).SetEase(Ease.OutCubic);
                        playerPosition.transform.DOMoveY(playerY, 0.25f / (2 / patternTime)).SetEase(Ease.InCubic).SetDelay(0.25f / (2 / patternTime));
                        playerPosition.transform.DOScale(new Vector3(0.5f, 0.5f, 0), 0.25f / (2 / patternTime)).SetEase(Ease.InCubic).SetDelay(0.25f / (2 / patternTime));
                        ErrorCount();
                    }
                    else
                    {
                        playerY -= 1;
                        floorNum += floorCol;
                        playerPosition.transform.DOMoveY(playerY + 1f, 0.5f / (2 / patternTime)).SetEase(Ease.OutCubic);
                        playerPosition.transform.DOScale(new Vector3(0.8f, 0.8f, 0), 0.5f / (2 / patternTime)).SetEase(Ease.OutCubic);
                        playerPosition.transform.DOMoveY(playerY, 0.5f / (2 / patternTime)).SetEase(Ease.InCubic).SetDelay(0.5f / (2 / patternTime));
                        playerPosition.transform.DOScale(new Vector3(0.5f, 0.5f, 0), 0.5f / (2 / patternTime)).SetEase(Ease.InCubic).SetDelay(0.5f / (2 / patternTime));
                        jumpSucessCount--;
                        jumpSucessText.text = "������ " + jumpSucessCount.ToString() + "�� �ϼ���";
                    }
                    break;
                case 2:
                    if (floorNum % floorCol == 0)
                    {
                        Debug.Log("���� Ʋ��");
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
                        jumpSucessText.text = "������ " + jumpSucessCount.ToString() + "�� �ϼ���";
                    }
                    break;
                case 3:
                    if (floorNum % floorCol == floorCol - 1)
                    {
                        Debug.Log("���� Ʋ��");
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
                        jumpSucessText.text = "������ " + jumpSucessCount.ToString() + "�� �ϼ���";
                    }
                    break;
            }
            cameraPosition.transform.DOMove(new Vector3(playerX, playerY, -10), 1f / (2 / patternTime));
            Invoke("FloorDamage", 1f / (2 / patternTime));
        }
        else // ��ư�� ������ Ÿ�̹��� �ƴ�
        {
            Debug.Log("Ÿ�̹� Ʋ��");
            ErrorCount();
        }
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

    void FloorAppear() // Ʃ�丮�� ���ӽ���
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
        Invoke("TutorialStart", 0.8f);
    }

    void TutorialStart()
    {
        PauseGame(1);
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
    void PatternStart()
    {
        sfxManager.PlaySound(11);
        Debug.Log("����");
        playerManager.PlayerStart(false);
        time = 0;
        playerTime = 0;
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
            for (int i = 0; i < buttonAnimations.Length; i++)
            {
                buttonAnimations[i].SetTrigger("isStart");
            }
        }
    }

    void ButtonOff()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].interactable = false;
        }

        for (int i = 0; i < buttonAnimations.Length; i++)
        {
            buttonAnimations[i].SetTrigger("isStop");
        }
    }

    void PauseGame(int i) // ��ȭâ ǥ��
    {
        sfxManager.PlaySound(10);
        talkPanel.SetActive(true);
        talkIndex = 0;

        talkID = i;
        talkText.text = talkManager.GetTalk(talkID, talkIndex++);
    }

    public void Talk() // ��ȭâ Ȯ�ι�ư
    {
        sfxManager.PlaySound(0);
        string talkData = talkManager.GetTalk(talkID, talkIndex);

        if (talkID == 1)
        {
            if (talkIndex == 3)
            {
                moveButtonsAnimator.SetTrigger("isMove");
            }
            else if (talkIndex == 4)
            {
                moveButtonsAnimator.SetTrigger("isStop");
            }
            else if (talkIndex == 8)
            {
                pauseButtonAnimator.SetTrigger("isMove");
            }
        }

        if (talkData == null) // ��ȭ ��
        {
            talkPanel.SetActive(false);

            if (talkID == 1) // Ʃ�丮��1 ����
            {
                pauseButtonAnimator.SetTrigger("isStop");
                CountDown3();
                Invoke("Tutorial1", 3f);
            }
            else if (talkID == 2) // Ʃ�丮��1 ����, �ٴ����� ����
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
            else if (talkID == 3) // Ʃ�丮��2 ����
            {
                FloorAppear();
                CountDown3();
                Invoke("Tutorial2", 3f);
            }
            else // Ʃ�丮��2 ����
            {
                sfxManager.PlaySound(8);
                cameraManager.Zoom(0);
                LoadingCanvasManager.Instance.ChangeScene("Main Scene");
            }

            return;
        }

        talkText.text = talkData;
        talkIndex++;
    }

    void Tutorial1()
    {
        time = 0;
        beatTime = 0;
        playerTime = 0;
        tutorial1 = true;
        jumpSucess.SetActive(true);
        jumpSucessText.text = "������ " + jumpSucessCount.ToString() + "�� �ϼ���";
    }

    void Tutorial1_End()
    {
        sfxManager.PlaySound(5);
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
        time = 0;
        beatTime = 0;
        playerTime = 0;
        tutorial2 = true;
        nextPattern = true;
        bgmManager.PlaySound(51, 0.5f);
    }

    public void PauseButton() // �Ͻ����� ��ư
    {
        sfxManager.PlaySound(0);
        Time.timeScale = 0;
        pause.SetActive(true);
        bgmManager.PauseSound();
    }

    public void ContinueButton() // ����ϱ� ��ư
    {
        sfxManager.PlaySound(0);
        Time.timeScale = 1;
        pause.SetActive(false);
        bgmManager.ContinueSound();
    }

    public void ExitButton() // ������ ��ư
    {
        PlayerPrefs.SetInt("Max Stage", 1);
        sfxManager.PlaySound(8);
        bgmManager.StopSound();
        ContinueButton();
        cameraManager.Zoom(0);
        LoadingCanvasManager.Instance.ChangeScene("Main Scene");
    }

    IEnumerator BackButton()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        backButton = true;
    }
}

using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
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
    public GameObject cameraBorderPosition;
    public BoxCollider2D cameraBorder;

    public GameObject pause;
    public float backButtonTime; // ����Ʈ�� �ڷΰ��� ��ư�� Ÿ�̸�
    public bool backButton; // ����Ʈ�� �ڷΰ��� ��ư Ŭ������ ����

    public Button[] buttons;
    public Animator[] countDowns;
    public Animator[] buttonAnimations;
    public Animator endAnimation;
    public Text stageNumText;
    public Text endText;
    public Text endScoreText;

    public bool start; // ��, �÷��̾� ����

    public bool buttonOn; // ��ư�� ������ Ÿ�̹� ����
    public bool buttonClick; // ��ư�� �������� ����
    public bool isPattern; // ���� ���� ����

    public int playerX; // �÷��̾� ��ġ
    public int playerY;

    public int floorRow; // ������ ��
    public int floorCol; // ������ ��
    public int floorNum; // �÷��̾ ���� ���� ��ȣ

    public bool colorHint; // ��ư ���� ��Ʈ (���� Ʋ��)
    public bool colorHint2; // ��ư ���� ��Ʈ (�ӵ� ��ȭ)
    public float beatTime; // ��ư ȿ������ �ð�
    public float time; // �������Ͽ� �ð�
    public float playerTime; // �÷��̾� Ÿ�ֿ̹� �ð�
    public float patternTime; // �����ֱ�
    public float patternAccuracy; // ���� ��Ȯ��
    public bool animationHint; // ���� ��Ʈ ���� (�̵���ư)

    public Text errorText;
    public int errorCount; // Ʋ�� Ƚ��
    public int cutLine; // Ŭ���� ĿƮ����
    public int stageNum; // ���� �������� ��ȣ

    public float bgmStartTime; // BGM ���۽ð�
    public float bgmSpeed; // BGM �ӵ�
    public bool patternStart; // ���� ����
    public bool nextPattern; // �������� ���ۿ���
    public int[] patternNums; // ���ϼ��� ����
    public int curPatternNum; // ���� ���ϼ���
    public int pattern; // ���� ���Ϲ�ȣ
    public int startNum; // ���� ���۹���
    public int isOdd; // Ȧ¦ �Ǻ�
    public int small; // �׵θ� ����

    public float curSpeed; // ���� ���� ���
    public Text curSpeedText; // ���� ���� ��� �ؽ�Ʈ
    public Animator speedAnimation;

    int n = 0;
    void Awake()
    {
        sfxManager = GameObject.Find("SFXManager").GetComponent<SFXManager>();
        bgmManager = GameObject.Find("BGMManager").GetComponent<BGMManager>();
        sfxManager.PlaySound(7);

        stageData = GameObject.Find("Stage Data").GetComponent<StageDataManager>();
        stageNum = stageData.stageDatas[0].stageNum;
        cutLine = stageData.stageDatas[0].cutLine;
        floorRow = stageData.stageDatas[0].floorRow;
        floorCol = stageData.stageDatas[0].floorCol;
        patternTime = stageData.stageDatas[0].patternTime;
        patternAccuracy = stageData.stageDatas[0].patternAccuracy;
        patternNums = stageData.stageDatas[0].patternNums;
        bgmStartTime = stageData.stageDatas[0].bgmStartTime;
        bgmSpeed = stageData.stageDatas[0].bgmSpeed;
        cameraBorderPosition.transform.position = new Vector3((float)(floorCol) / 2 - 0.5f, -1 * ((float)(floorRow) / 2 - 0.5f), 0);
        cameraBorder.size = new Vector2(floorCol + 2, floorRow + 2);

        stageNumText.text += stageNum.ToString();
        errorText.text = errorCount.ToString();
        playerX = (floorCol - 1) / 2;
        playerY = -1 * (floorRow - 1) / 2;
        floorNum = playerX + -1 * playerY * floorCol;
        
        cameraPosition.transform.position = new Vector3(playerX, playerY, -10);
        playerPosition.transform.position = new Vector3(playerX, playerY + 10);

        for (int i = 0; i < floorRow * floorCol; i++)
        {
            floorsPosition[i].SetActive(true);
            floorsPosition[i].transform.position = new Vector3(i % floorCol, -1 * i / floorCol + 10);
            floors[i].PatternTime(patternTime);
        }

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

        time += Time.deltaTime;
        playerTime += Time.deltaTime;

        if (start)
        {
            if (time > 0.5f)
            {
                for (int i = 0; i < floorRow * floorCol; i++) // �� ����
                {
                    if ((i / floorCol >= startNum && i / floorCol < floorRow - startNum
                        && i % floorCol >= startNum && i % floorCol < floorCol - startNum)
                        && (i % floorCol == startNum || i % floorCol == floorCol - 1 - startNum
                        || i < floorCol * (1 + startNum) || (i > floorCol * (floorRow - 1 - startNum))))
                    {
                        floors[i].FloorStart(i, floorCol);
                    }
                }
                time -= 0.5f;
                startNum++;
            }
            if (startNum > small) // �÷��̾� ����
            {
                playerManager.PlayerPosition(playerX, playerY);
                start = false;
                Invoke("CountDown3", 0.5f);
            }
        }

        if (patternStart)
        {
            beatTime += Time.deltaTime;

            if (beatTime > 0.5f)
            {
                beatTime -= 0.5f;
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
                if (n % 2 > 0)
                    Debug.Log("��ư Ȱ��ȭ, Ȧ");
                else
                    Debug.Log("��ư Ȱ��ȭ, ¦");
                n++;
                buttonOn = true;
                isPattern = true;

                if (colorHint2)
                {
                    for (int i = 0; i < buttons.Length; i++)
                    {
                        buttons[i].image.DOColor(new Color(0, 1, 0), patternTime * patternAccuracy);
                        buttons[i].image.DOColor(new Color(1, 1, 1), patternTime * patternAccuracy).SetDelay(patternTime * patternAccuracy * 2);
                    }
                    colorHint = false;
                    colorHint2 = false;
                }
                else if (colorHint)
                {
                    for (int i = 0; i < buttons.Length; i++)
                    {
                        buttons[i].image.DOColor(new Color(1, 0, 0), patternTime * patternAccuracy);
                        buttons[i].image.DOColor(new Color(1, 1, 1), patternTime * patternAccuracy).SetDelay(patternTime * patternAccuracy * 2);
                    }
                    colorHint = false;
                    colorHint2 = false;
                }
            }
            
            // ������ �ð��ʰ�
            if (playerTime > patternTime * (1 + patternAccuracy))
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
                            stageNum++;
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
                    startNum = small - 1;
                }
                else if (pattern < 12)
                {
                    startNum = 1;
                }
                else
                {
                    startNum = 0;
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
                        if (startNum >= small)
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
                            if (floorCol % 2 < 1)
                            {
                                if ((i / floorCol) % 2 < 1)
                                {
                                    if (i % 2 == isOdd % 2)
                                    {
                                        floorPattern(i);
                                    }

                                }
                                else
                                {
                                    if (i % 2 == (isOdd + 1) % 2)
                                    {
                                        floorPattern(i);
                                    }
                                }
                            }
                            else
                            {
                                if (i % 2 == isOdd % 2)
                                {
                                    floorPattern(i);
                                }
                            }
                        }
                        isOdd = (isOdd + 1) % 2;
                        nextPattern = true;
                        break;
                    case 12: // ���� ����
                        for (int i = 0; i < floorRow; i++)
                        {
                            for (int j = 0; j <= (floorCol - 1) / 2; j++)
                            {
                                floorPattern(j + i * floorCol);
                            }
                        }
                        nextPattern = true;
                        break;
                    case 13: // ������ ����
                        for (int i = 0; i < floorRow; i++)
                        {
                            for (int j = floorCol - 1; j >= floorCol / 2; j--)
                            {
                                floorPattern(j + i * floorCol);
                            }
                        }
                        nextPattern = true;
                        break;
                    case 14: // ���� ����
                        for (int i = 0; i < floorCol; i++)
                        {
                            for (int j = 0; j <= (floorRow - 1) / 2; j++)
                            {
                                floorPattern(j * floorCol + i);
                            }
                        }
                        nextPattern = true;
                        break;
                    case 15: // �Ʒ��� ����
                        for (int i = 0; i < floorCol; i++)
                        {
                            for (int j = floorRow - 1; j >= floorRow / 2; j--)
                            {
                                floorPattern(j * floorCol + i);
                            }
                        }
                        nextPattern = true;
                        break;
                    case 16: // ���� ����
                        for (int i = 0; i < floorRow; i++)
                        {
                            floorPattern(startNum + i * floorCol);
                            floorPattern(floorCol - startNum - 1 + i * floorCol);
                        }
                        startNum += 1;
                        if (startNum >= (floorCol + 1) / 2)
                        {
                            nextPattern = true;
                        }
                        break;
                    case 17: // ���� ����
                        for (int i = 0; i < floorCol; i++)
                        {
                            floorPattern(startNum + i);
                            floorPattern((floorRow - 1) * floorCol - startNum + i);
                        }
                        startNum += floorCol;
                        if (startNum / floorCol >= (floorRow + 1) / 2)
                        {
                            nextPattern = true;
                        }
                        break;
                    case 212: // 2�� ������ �غ�
                        speedAnimation.SetFloat("animationSpeed", 1 / patternTime);
                        speedAnimation.SetTrigger("isSpeedChange");
                        SpeedChange(2, false);
                        nextPattern = true;
                        break;
                    case 112: // 2�� ������
                        curSpeed /= 2;
                        buttonOn = buttonClick = false;
                        patternTime *= 2;
                        patternAccuracy /= 2;
                        for (int i = 0; i < floorRow * floorCol; i++)
                        {
                            floors[i].PatternTime(patternTime);
                        }
                        nextPattern = true;
                        colorHint2 = true;
                        break;
                    case 202: // 2�� ������ �غ�
                        speedAnimation.SetFloat("animationSpeed", 1 / patternTime);
                        speedAnimation.SetTrigger("isSpeedChange");
                        SpeedChange(2);
                        nextPattern = true;
                        break;
                    case 102: // 2�� ������
                        curSpeed *= 2;
                        buttonOn = buttonClick = false;
                        patternTime /= 2;
                        patternAccuracy *= 2;
                        for (int i = 0; i < floorRow * floorCol; i++)
                        {
                            floors[i].PatternTime(patternTime);
                        }
                        nextPattern = true;
                        colorHint2 = true;
                        break;
                    case 213: // 3�� ������ �غ�
                        speedAnimation.SetFloat("animationSpeed", 1 / patternTime);
                        speedAnimation.SetTrigger("isSpeedChange");
                        SpeedChange(3, false);
                        nextPattern = true;
                        break;
                    case 113: // 3�� ������
                        curSpeed /= 3;
                        buttonOn = buttonClick = false;
                        patternTime *= 3;
                        patternAccuracy /= 3;
                        for (int i = 0; i < floorRow * floorCol; i++)
                        {
                            floors[i].PatternTime(patternTime);
                        }
                        nextPattern = true;
                        colorHint2 = true;
                        break;
                    case 203: // 3�� ������ �غ�
                        speedAnimation.SetFloat("animationSpeed", 1 / patternTime);
                        speedAnimation.SetTrigger("isSpeedChange");
                        SpeedChange(3);
                        nextPattern = true;
                        break;
                    case 103: // 3�� ������
                        curSpeed *= 3;
                        buttonOn = buttonClick = false;
                        patternTime /= 3;
                        patternAccuracy *= 3;
                        for (int i = 0; i < floorRow * floorCol; i++)
                        {
                            floors[i].PatternTime(patternTime);
                        }
                        nextPattern = true;
                        colorHint2 = true;
                        break;
                    case 214: // 4�� ������ �غ�
                        speedAnimation.SetFloat("animationSpeed", 1 / patternTime);
                        speedAnimation.SetTrigger("isSpeedChange");
                        SpeedChange(4, false);
                        nextPattern = true;
                        break;
                    case 114: // 4�� ������
                        curSpeed /= 4;
                        buttonOn = buttonClick = false;
                        patternTime *= 4;
                        patternAccuracy /= 4;
                        for (int i = 0; i < floorRow * floorCol; i++)
                        {
                            floors[i].PatternTime(patternTime);
                        }
                        nextPattern = true;
                        colorHint2 = true;
                        break;
                    case 204: // 4�� ������ �غ�
                        speedAnimation.SetFloat("animationSpeed", 1 / patternTime);
                        speedAnimation.SetTrigger("isSpeedChange");
                        SpeedChange(4);
                        nextPattern = true;
                        break;
                    case 104: // 4�� ������
                        curSpeed *= 4;
                        buttonOn = buttonClick = false;
                        patternTime /= 4;
                        patternAccuracy *= 4;
                        for (int i = 0; i < floorRow * floorCol; i++)
                        {
                            floors[i].PatternTime(patternTime);
                        }
                        nextPattern = true;
                        colorHint2 = true;
                        break;
                    case 215: // 5�� ������ �غ�
                        speedAnimation.SetFloat("animationSpeed", 1 / patternTime);
                        speedAnimation.SetTrigger("isSpeedChange");
                        SpeedChange(5, false);
                        nextPattern = true;
                        break;
                    case 115: // 5�� ������
                        curSpeed /= 5;
                        buttonOn = buttonClick = false;
                        patternTime *= 5;
                        patternAccuracy /= 5;
                        for (int i = 0; i < floorRow * floorCol; i++)
                        {
                            floors[i].PatternTime(patternTime);
                        }
                        nextPattern = true;
                        colorHint2 = true;
                        break;
                    case 205: // 5�� ������ �غ�
                        speedAnimation.SetFloat("animationSpeed", 1 / curSpeed);
                        speedAnimation.SetTrigger("isSpeedChange");
                        SpeedChange(5);
                        nextPattern = true;
                        break;
                    case 105: // 5�� ������
                        curSpeed *= 5;
                        buttonOn = buttonClick = false;
                        patternTime /= 5;
                        patternAccuracy *= 5;
                        for (int i = 0; i < floorRow * floorCol; i++)
                        {
                            floors[i].PatternTime(patternTime);
                        }
                        nextPattern = true;
                        colorHint2 = true;
                        break;
                }
            }
        }
    }

    public void SpeedChange(int n = 1, bool fast = true)
    {
        float speed;
        if (fast)
        {
            speed = curSpeed * n;
        }
        else
        {
            speed = curSpeed / n;
        }

        if (speed >= 1)
        {
            curSpeedText.text = "��" + speed.ToString();
        }
        else
        {
            curSpeedText.text = "��" + (1f / speed).ToString();
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
                    }
                    break;
            }
            cameraPosition.transform.DOMove(new Vector3(playerX, playerY, -10), 1f / (2 / patternTime));
            Invoke("FloorDamage", 1f / (2 / patternTime));
        }
        else // ��ư�� ������ Ÿ�̹��� �ƴ�
        {
            Debug.Log("Ÿ�̹� Ʋ��");
            colorHint = true;
            ErrorCount();
        }
    }

    void FloorDamage()
    {
        if (floors[floorNum].damage > 0) // �÷��̾ ���� ������ ������ ������
        {
            Debug.Log("���� ����");
            ErrorCount();
            explosion[floorNum].SetTrigger("isExplosion");
            sfxManager.PlaySound(3);
        }
    }

    void MapStart()
    {
        cameraManager.Zoom(5);
        start = true;
        time = 0;
        playerTime = 0;
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
        Debug.Log("����");
        sfxManager.PlaySound(11);
        bgmManager.PlaySound(bgmManager.curStageNum, bgmStartTime, bgmSpeed);
        playerManager.PlayerStart(false);
        patternStart = true;
        nextPattern = true;
        time = 0;
        playerTime = 0;

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

    public void StageEnd()
    {
        sfxManager.PlaySound(8);
        endAnimation.SetTrigger("isUp");
        cameraManager.Zoom(0);
        LoadingCanvasManager.Instance.ChangeScene("Main Scene");
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
        patternStart = false;
        sfxManager.PlaySound(8);
        bgmManager.NullSound();
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

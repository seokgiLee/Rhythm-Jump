using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
    public GameObject cameraPosition;
    public GameObject playerPosition;
    public GameObject[] floorsPosition;
    public FloorManager[] floors;
    public PlayerManager playerManager;

    public Button[] buttons;
    public Animator[] countDowns;

    public Animator[] ButtonAnimators;

    public bool start; // ��, �÷��̾� ����

    public bool buttonOn; // ��ư�� ������ Ÿ�̹� ����
    public bool buttonClick; // ��ư�� �������� ����

    public int playerX; // �÷��̾� ��ġ
    public int playerY;

    public int floorRow; // ������ ��
    public int floorCol; // ������ ��
    public int floorNum; // �÷��̾ ���� ���� ��ȣ

    public float time; // �������Ͽ� �ð�
    public float patternTime; // �����ֱ�
    public float patternAccuracy; // ���� ��Ȯ��
    public bool animationHint; // ���� ��Ʈ ���� (�̵���ư)

    public Text errorText;
    public int errorCount; // Ʋ�� Ƚ��

    public bool patternStart; // ���� ����
    public bool nextPattern; // �������� ���ۿ���
    public int[] patternNums; // ���ϼ��� ����
    public int[] pattenIntervals; // ���ϻ��� ���� ����
    public int curPatternNum; // ���� ���ϼ���
    public int pattern; // ���� ���Ϲ�ȣ
    public int startNum; // ���� ���۹���
    public int small; // �׵θ� ����

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
            floorsPosition[i].SetActive(true);
            floorsPosition[i].transform.position = new Vector3(i % floorCol, -1 * i / floorCol + 10);
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

        Invoke("MapStart", 1);
    }

    void Update()
    {
        time += Time.deltaTime;
        
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
            }

            if (floors[floorNum].damage) // �÷��̾ ���� ������ ������ ������
            {
                Debug.Log("���� ����");
                ErrorCount();
                floors[floorNum].damage = false;
            }

            if (nextPattern)
            {
                nextPattern = false;
                if (curPatternNum >= patternNums.Length)
                {
                    // �� ����
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
                case 0: // ����Ÿ��
                    if (time > patternTime)
                    {
                        time -= patternTime;
                        nextPattern = true;
                        if(buttonClick)
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
            buttonOn = false;
            buttonClick = true;
            switch (n)
            {
                case 0:
                    if (floorNum - floorCol < 0)
                    {
                        Debug.Log("���� Ʋ��");
                        ErrorCount();
                    }
                    else
                    {
                        playerY += 1;
                        floorNum -= floorCol;
                    }
                    break;
                case 1:
                    if (floorNum + floorCol >= floorRow * floorCol)
                    {
                        Debug.Log("���� Ʋ��");
                        ErrorCount();
                    }
                    else
                    {
                        playerY -= 1;
                        floorNum += floorCol;
                    }
                    break;
                case 2:
                    if (floorNum % floorCol == 0)
                    {
                        Debug.Log("���� Ʋ��");
                        ErrorCount();
                    }
                    else
                    {
                        playerX -= 1;
                        floorNum -= 1;
                    }
                    break;
                case 3:
                    if (floorNum % floorCol == floorCol - 1)
                    {
                        Debug.Log("���� Ʋ��");
                        ErrorCount();
                    }
                    else
                    {
                        playerX += 1;
                        floorNum += 1;
                    }
                    break;
            }
            playerPosition.transform.position = new Vector3(playerX, playerY);
        }
        else // ��ư�� ������ Ÿ�̹��� �ƴ�
        {
            Debug.Log("Ÿ�̹� Ʋ��");
            ErrorCount();
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
    void PatternStart()
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

        if (animationHint)
        {
            for (int i = 0; i < ButtonAnimators.Length; i++)
            {
                ButtonAnimators[i].SetTrigger("isStart");
            }
        }
    }
}

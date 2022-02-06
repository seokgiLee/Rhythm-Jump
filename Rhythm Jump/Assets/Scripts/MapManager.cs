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

    public int floorRow; // ������ ��
    public int floorCol; // ������ ��
    public int floorNum; // �÷��̾ ���� ���� ��ȣ

    public float time; // �������Ͽ� �ð�
    public float patternTime; // �����ֱ�

    public Text errorText;
    public int errorCount; // Ʋ�� Ƚ��

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
        int x = (floorCol - 1) / 2;
        int y = (floorRow - 1) / 2;
        floorNum = x + y * floorCol;
        cameraPosition.transform.position = new Vector3(x, -1 * y, -10);
        playerPosition.transform.position = new Vector3(x, -1 * y);
        playerManager.x = x;
        playerManager.y = -1 * y;
        playerManager.patternTime = patternTime;
        for (int i = 0; i < floorRow * floorCol; i++)
        {
            floorsPosition[i].SetActive(true);
            floorsPosition[i].transform.position = new Vector3(i % floorCol, -1 * i / floorCol);
            floors[i].TimeScale();
        }
    }

    void Update()
    {
        time += Time.deltaTime;

        if (curPatternNum >= patternNums.Length)
        {
            // �� ����
        }

        if (nextPattern)
        {
            nextPattern = false;
            if (curPatternNum >= patternNums.Length)
            {
                // �� ����
                pattern = 0;
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
            else if (pattern < 11)
            {
                if (floorRow > floorCol)
                {
                    small = (floorCol + 1) / 2;
                    startNum = 0;
                }
                else
                {
                    small = (floorRow + 1) / 2;
                    startNum = small;
                }
            }
            else if (pattern < 12)
            {
                startNum = 1;
            }
        }

        switch (pattern)
        {
            case 0: // ����Ÿ��
                if (time > patternTime * 3)
                {
                    time -= patternTime * 3;
                    nextPattern = true;
                }
                break;
            case 1: // ó������ ������ ���� �������
                if (time > patternTime)
                {
                    floorPattern(startNum++);
                    time -= patternTime;
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
                }
                if (startNum > 10)
                {
                    nextPattern = true;
                }
                break;
        }

        if (floors[floorNum].damage) // �÷��̾ ���� ������ ������ ������
        {
            ErrorCount();
            floors[floorNum].damage = false;
        }
    }

    public void floorPattern(int n) // ���� ���� ����
    {
        floors[n].floorPatternStart();
    }

    public void ErrorCount()
    {
        errorCount++;
        errorText.text = errorCount.ToString();
    }
}

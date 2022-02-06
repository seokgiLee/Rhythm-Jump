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

    public int floorRow; // 발판의 행
    public int floorCol; // 발판의 열
    public int floorNum; // 플레이어가 밟은 발판 번호

    public float time; // 발판패턴용 시간
    public float patternTime; // 패턴주기

    public Text errorText;
    public int errorCount; // 틀린 횟수

    public bool nextPattern; // 다음패턴 시작여부
    public int[] patternNums; // 패턴순서 모음
    public int[] pattenIntervals; // 패턴사이 간격 모음
    public int curPatternNum; // 현재 패턴순서
    public int pattern; // 현재 패턴번호
    public int startNum; // 패턴 시작발판
    public int small; // 테두리 개수

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
            // 맵 종료
        }

        if (nextPattern)
        {
            nextPattern = false;
            if (curPatternNum >= patternNums.Length)
            {
                // 맵 종료
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
            case 0: // 공백타임
                if (time > patternTime * 3)
                {
                    time -= patternTime * 3;
                    nextPattern = true;
                }
                break;
            case 1: // 처음부터 끝까지 가로 순서대로
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
                }
                if (startNum > 10)
                {
                    nextPattern = true;
                }
                break;
        }

        if (floors[floorNum].damage) // 플레이어가 검은 발판을 밟으면 데미지
        {
            ErrorCount();
            floors[floorNum].damage = false;
        }
    }

    public void floorPattern(int n) // 발판 패턴 시작
    {
        floors[n].floorPatternStart();
    }

    public void ErrorCount()
    {
        errorCount++;
        errorText.text = errorCount.ToString();
    }
}

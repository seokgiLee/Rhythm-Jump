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
    public float backButtonTime; // ����Ʈ�� �ڷΰ��� ��ư�� Ÿ�̸�
    public bool backButton; // ����Ʈ�� �ڷΰ��� ��ư Ŭ������ ����

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
                if (pause.activeSelf) // �Ͻ�����â�� ���������� ����ϱ�
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
            buttonOn = false;
            buttonClick = true;
            switch (n)
            {
                case 0:
                    if (floorNum - floorCol < 0)
                    {
                        Debug.Log("���� Ʋ��");
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
                    }
                    break;
                case 1:
                    if (floorNum + floorCol >= floorRow * floorCol)
                    {
                        Debug.Log("���� Ʋ��");
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
        }
    }

    void TutorialStart() // Ʃ�丮�� ���ӽ���
    {
        for (int i = 0; i < floorRow * floorCol; i++)
        {
            if ((i / floorCol >= startNum && i / floorCol < floorRow - startNum
                && i % floorCol >= startNum && i % floorCol < floorCol - startNum)
                && (i % floorCol == startNum || i % floorCol == floorCol - 1 - startNum
                || i < floorCol * (1 + startNum) || (i > floorCol * (floorRow - 1 - startNum))))
            {
                floorsPosition[i].SetActive(true);
                floorsPosition[i].transform.position = new Vector3(i % floorCol, -1 * i / floorCol + 10);
                floors[i].PatternTime();
            }
        }
    }

    void PlayerAppear()
    {
        playerManager.PlayerPosition(playerX, playerY);
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

    public void PauseButton() // �Ͻ����� ��ư
    {
        Time.timeScale = 0;
        pause.SetActive(true);
    }

    public void ContinueButton() // ����ϱ� ��ư
    {
        Time.timeScale = 1;
        pause.SetActive(false);
    }

    public void ExitButton() // ������ ��ư
    {
        SceneManager.LoadScene("Main Scene");
    }
}
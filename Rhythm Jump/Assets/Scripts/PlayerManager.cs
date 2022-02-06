using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public GameManager gameManager;
    Transform playerPosition;
    SpriteRenderer playerImage;
    public Sprite[] playerImages;
    public MapManager mapManager;

    public float time; // ����ð�
    public float patternTime; // �����ֱ�

    public Button[] buttons; // ���۹�ư
    int backJump = 0; // 0: �Ϲ� ����, 4: �ڷ� ����
    public float x; // �÷��̾� ��ġ
    public float y;

    public bool jump; // ������ư Ŭ�� ����
    public bool buttonOn; // ��ư�� ������ Ÿ�̹� ����

    void Awake()
    {
        playerImage = GetComponent<SpriteRenderer>();
        playerPosition = GetComponent<Transform>();
    }

    void Update()
    {
        time += Time.deltaTime;

        if (time > patternTime * 0.8f) // ������ �ð��� 80%�� ����
        {
            if (!jump) // ��ư�� �ȴ������� ��ư Ȱ��ȭ
            {
                if (!buttonOn)
                    Debug.Log("��ư Ȱ��ȭ");
                buttonOn = true;
            }

            if (jump && time > patternTime) // ��ư�� �������� time�� ������ �ð���ŭ ����
            {
                jump = false;
                time -= patternTime;
            }

            if (time > patternTime * 1.2f) // ������ �ð��� 120%�� ����
            {
                mapManager.ErrorCount();
                time -= patternTime;
                buttonOn = false;
            }
        }
    }

    public void MoveButton(int n)
    {
        if (buttonOn) // ��ư�� ������ Ÿ�̹�
        {
            jump = true;
            buttonOn = false;

            switch (n)
            {
                case 0:
                    y += 1;
                    mapManager.floorNum -= mapManager.floorCol;
                    break;
                case 1:
                    y -= 1;
                    mapManager.floorNum += mapManager.floorCol;
                    break;
                case 2:
                    x -= 1;
                    mapManager.floorNum -= 1;
                    break;
                case 3:
                    x += 1;
                    mapManager.floorNum += 1;
                    break;
            }
            playerPosition.position = new Vector3(x, y);
        }
        else // ��ư�� ������ Ÿ�̹��� �ƴ�
        {
            mapManager.ErrorCount();
        }
    }
}

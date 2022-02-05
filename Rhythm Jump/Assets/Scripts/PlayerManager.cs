using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject playerPosition;
    SpriteRenderer playerImage;
    Animator playerAnimation;
    public Sprite[] playerImages;
    public MapManager mapManager;

    public float time; // ����ð�
    public float timeScale; // �ð����� ����

    public int direction; // 0: ��, 1: ��, 2: ��, 3: ��
    public Button[] buttons; // ���۹�ư
    int backJump = 0; // 0: �Ϲ� ����, 4: �ڷ� ����
    float x, y; // �÷��̾� ��ġ

    public bool jump; // ������ư Ŭ�� ����
    public bool buttonOn; // ��ư�� ������ Ÿ�̹� ����

    void Awake()
    {
        playerImage = GetComponent<SpriteRenderer>();
        playerAnimation = GetComponent<Animator>();
        x = playerPosition.transform.position.x;
        y = playerPosition.transform.position.y;
    }

    void Update()
    {
        time += Time.deltaTime;

        if (time > (2 / timeScale) * 0.8f) // ������ �ð��� 80%�� ����
        {
            if (!jump) // ��ư�� �ȴ������� ��ư Ȱ��ȭ
            {
                if (!buttonOn)
                    Debug.Log("��ư Ȱ��ȭ");
                buttonOn = true;
            }

            if (jump && time > (2 / timeScale)) // ��ư�� �������� time�� ������ �ð���ŭ ����
            {
                jump = false;
                time -= 2 / timeScale;
            }

            if (time > (2 / timeScale) * 1.2f) // ������ �ð��� 120%�� ����
            {
                mapManager.ErrorCount();
                time -= 2 / timeScale;
                buttonOn = false;
            }
        }
    }

    public void Move()
    {
        playerAnimation.SetInteger("PlayerDirection", -1);
        switch (direction)
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
        playerPosition.transform.position = new Vector3(x, y);
    }

    public void MoveButton(int n)
    {
        if (buttonOn) // ��ư�� ������ Ÿ�̹�
        {
            direction = n;
            playerAnimation.SetInteger("PlayerDirection", direction + backJump);

            jump = true;
            buttonOn = false;
        }
        else // ��ư�� ������ Ÿ�̹��� �ƴ�
        {
            mapManager.ErrorCount();
            PlayerSprite(n);
        }
    }

    public void PlayerSprite(int n)
    {
        playerImage.sprite = playerImages[n];
    }
}

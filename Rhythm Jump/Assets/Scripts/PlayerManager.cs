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

    public float time; // 현재시간
    public float timeScale; // 시간증감 변수

    public int direction; // 0: 상, 1: 하, 2: 좌, 3: 우
    public Button[] buttons; // 조작버튼
    int backJump = 0; // 0: 일반 점프, 4: 뒤로 점프
    float x, y; // 플레이어 위치

    public bool jump; // 점프버튼 클릭 여부
    public bool buttonOn; // 버튼을 누르는 타이밍 여부

    public int errorCount; // 틀린 횟수

    public MapManager[] maps; // 발판
    public int row; // 발판의 행
    public int col; // 발판의 열

    public float time2;
    int n = 0;

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

        if (time > (2 / timeScale) * 0.8f) // 정해진 시간의 80%에 도달
        {
            if (!jump) // 버튼을 안눌렀으면 버튼 활성화
            {
                if (!buttonOn)
                    Debug.Log("버튼 활성화");
                buttonOn = true;
            }

            if (jump && time > (2 / timeScale)) // 버튼을 눌렀으면 time을 정해진 시간만큼 제거
            {
                jump = false;
                time -= 2 / timeScale;
            }

            if (time > (2 / timeScale) * 1.2f) // 정해진 시간의 120%에 도달
            {
                // 데미지
                time -= 2 / timeScale;
                buttonOn = false;
            }
        }

        time2 += Time.deltaTime;

        if (time2 > 2 / timeScale)
        {
            MapPattern(n % maps.Length);
            n++;
            time2 -= 2;
        }
    }

    public void Move()
    {
        playerAnimation.SetInteger("PlayerDirection", -1);
        switch (direction)
        {
            case 0:
                y += 1;
                break;
            case 1:
                y -= 1;
                break;
            case 2:
                x -= 1;
                break;
            case 3:
                x += 1;
                break;
        }
        playerPosition.transform.position = new Vector3(x, y);
    }

    public void MoveButton(int n)
    {
        if (buttonOn) // 버튼을 누르는 타이밍
        {
            direction = n;
            playerAnimation.SetInteger("PlayerDirection", direction + backJump);

            jump = true;
            buttonOn = false;
        }
        else // 버튼을 누르는 타이밍이 아님
        {
            PlayerSprite(n);
        }
    }

    public void PlayerSprite(int n)
    {
        playerImage.sprite = playerImages[n];
    }

    public void MapPattern(int n)
    {
        maps[n].MapPattern1();
    }
}

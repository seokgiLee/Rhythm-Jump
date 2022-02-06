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

    public float time; // 현재시간
    public float patternTime; // 패턴주기

    public Button[] buttons; // 조작버튼
    int backJump = 0; // 0: 일반 점프, 4: 뒤로 점프
    public float x; // 플레이어 위치
    public float y;

    public bool jump; // 점프버튼 클릭 여부
    public bool buttonOn; // 버튼을 누르는 타이밍 여부

    void Awake()
    {
        playerImage = GetComponent<SpriteRenderer>();
        playerPosition = GetComponent<Transform>();
    }

    void Update()
    {
        time += Time.deltaTime;

        if (time > patternTime * 0.8f) // 정해진 시간의 80%에 도달
        {
            if (!jump) // 버튼을 안눌렀으면 버튼 활성화
            {
                if (!buttonOn)
                    Debug.Log("버튼 활성화");
                buttonOn = true;
            }

            if (jump && time > patternTime) // 버튼을 눌렀으면 time을 정해진 시간만큼 제거
            {
                jump = false;
                time -= patternTime;
            }

            if (time > patternTime * 1.2f) // 정해진 시간의 120%에 도달
            {
                mapManager.ErrorCount();
                time -= patternTime;
                buttonOn = false;
            }
        }
    }

    public void MoveButton(int n)
    {
        if (buttonOn) // 버튼을 누르는 타이밍
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
        else // 버튼을 누르는 타이밍이 아님
        {
            mapManager.ErrorCount();
        }
    }
}

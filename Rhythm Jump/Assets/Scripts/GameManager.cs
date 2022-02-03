using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    SpriteRenderer playerImage;
    Animator playerAnimation;
    public Sprite[] playerImages;

    public float time;
    public float timeScale;

    public int direction; // 0: ��, 1: ��, 2: ��, 3: ��
    public Button[] buttons; // ���۹�ư
    int backJump = 0; // 0: �Ϲ� ����, 4: �ڷ� ����

    void Awake()
    {
        playerImage = player.GetComponent<SpriteRenderer>();
        playerAnimation = player.GetComponent<Animator>();
        Move(1);
    }

    void Update()
    {
        time += Time.deltaTime;
        if (time > 1 / timeScale)
        {
            time = 0;

            playerAnimation.SetInteger("PlayerDirection", direction + backJump);
            switch (direction + backJump)
            {
                case 0:
                    player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 1);
                    break;
                case 1:
                    player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y - 1);
                    break;
                case 2:
                    player.transform.position = new Vector3(player.transform.position.x - 1, player.transform.position.y);
                    break;
                case 3:
                    player.transform.position = new Vector3(player.transform.position.x + 1, player.transform.position.y);
                    break;
            }
        }
    }

    public void Move(int n)
    {
        direction = n;

        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].image.color = new Color(1, 1, 1);
            buttons[i].interactable = true;
        }

        buttons[n].image.color = new Color(1, 1, 0);
        buttons[n].interactable = false;

        playerImage.sprite = playerImages[n];
    }
}

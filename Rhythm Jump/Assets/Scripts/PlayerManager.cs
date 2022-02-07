using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    bool start = false;
    public float playerX; // 플레이어 위치
    public float playerY;

    void Update()
    {
        if (start)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(playerX, playerY), Time.deltaTime * 50);
        }
    }

    public void PlayerPosition(float x,float y)
    {
        PlayerStart(true);
        playerX = x;
        playerY = y;
    }

    public void PlayerStart(bool s)
    {
        start = s;
    }
}

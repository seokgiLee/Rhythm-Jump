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
    public float timeScale; // 시간증감 변수
    int n = 0; // 발판 번호

    public Text errorText;
    public int errorCount; // 틀린 횟수

    void Awake()
    {
        errorText.text = errorCount.ToString();
        int x = (floorRow - 1) / 2;
        int y = (floorCol - 1) / 2;
        floorNum = x * floorCol + y;
        cameraPosition.transform.position = new Vector3(x, -1 * y, -10);
        playerPosition.transform.position = new Vector3(x, -1 * y + 0.5f);
        for (int i = 0; i < floorRow * floorCol; i++)
        {
            floorsPosition[i].SetActive(true);
            floorsPosition[i].transform.position = new Vector3(i % floorCol, -1 * i / floorRow);
            floors[i].TimeScale();
        }
    }

    void Update()
    {
        time += Time.deltaTime;

        // 패턴 만들기
        // 세로줄 순서대로 2방향
        // 가로줄 순서대로 2방향
        // 대각선 순서대로 4방향
        // 가운데에서 순서대로
        // 가장자리에서 순서대로
        // 홀짝 순서대로
        // 랜덤
        if (time > 2 / timeScale)
        {
            // 순서대로
            floorPattern(n % floorRow * floorCol);
            n++;
            time -= 2 / timeScale;
        }

        if (floors[floorNum].damage)
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

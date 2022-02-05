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
    public float timeScale; // �ð����� ����
    int n = 0; // ���� ��ȣ

    public Text errorText;
    public int errorCount; // Ʋ�� Ƚ��

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

        // ���� �����
        // ������ ������� 2����
        // ������ ������� 2����
        // �밢�� ������� 4����
        // ������� �������
        // �����ڸ����� �������
        // Ȧ¦ �������
        // ����
        if (time > 2 / timeScale)
        {
            // �������
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

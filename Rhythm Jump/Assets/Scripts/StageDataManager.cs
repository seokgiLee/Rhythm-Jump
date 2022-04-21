using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageDataManager : MonoBehaviour
{
    public List<StageData> stageDatas;

    void Awake()
    {
        // 0: 공백타임
        // 1: 처음부터 끝까지 가로 순서대로
        // 2: 처음부터 끝까지 세로 순서대로
        // 3: 끝부터 처음까지 가로 순서대로
        // 4: 끝부터 처음까지 세로 순서대로
        // 5: 세로줄 왼쪽 순서대로
        // 6: 세로줄 오른쪽 순서대로
        // 7: 가로줄 위 순서대로
        // 8: 가로줄 아래 순서대로
        // 9: 가장자리에서 순서대로
        // 10: 가운데에서 순서대로
        // 11: 홀짝
        // 101: 2배 느리게
        // 102: 2배 빠르게

        stageDatas = new List<StageData>();
        stageDatas.Add(null);
        stageDatas.Add(new StageData(1, 30, 2, 3, 2f, 0.3f, new int[] { 5, 0, 6, 0, 5, 0, 6, 0, 0, 7, 0, 8, 0, 7, 0, 0, 5, 0, 0, 7, 0, 0, 6, 0, 0, 8, 0, 0 }, 0f));
        stageDatas.Add(new StageData(2, 25, 2, 3, 2f, 0.3f, new int[] { 1, 0, 3, 0, 5, 0, 6, 0, 0, 7, 0, 8, 0, 0 }, 0f));
        stageDatas.Add(new StageData(3, 25, 2, 4, 2f, 0.3f, new int[] { 0, 11, 11, 0, 5, 0, 6, 0, 5, 0, 6, 0, 0, 11, 11, 0, 5, 0, 6, 0, 5, 0, 6, 0, 0, 11, 11, 0, 5, 0, 6, 0, 5, 0, 6, 0, 0, 11, 11, 11, 11, 0, 0 }, 0));
        stageDatas.Add(new StageData(4, 25, 4, 2, 2f, 0.3f, new int[] { 8, 0, 0, 11, 11, 0, 8, 0, 0, 11, 11, 0, 7, 0, 0, 11, 11, 11, 0, 8, 0, 0, 11, 11, 0, 8, 0, 6, 0, 5, 0, 6, 0, 5, 0, 0 }, 8f));
        stageDatas.Add(new StageData(5, 25, 3, 3, 2f, 0.3f, new int[] { 9, 0, 10, 0, 9, 0, 10, 0, 11, 11, 11, 11, 11, 11, 11, 11, 0, 0, 9, 0, 10, 0, 0 }, 0.1f, 0.8f));
        stageDatas.Add(new StageData(6, 25, 3, 2, 2f, 0.3f, new int[] { 5, 0, 5, 0, 6, 0, 6, 0, 5, 0, 5, 0, 6, 0, 6, 0, 5, 0, 5, 0, 5, 0, 6, 0, 6, 0, 6, 0, 7, 0, 0 }, 0));
        stageDatas.Add(new StageData(7, 25, 3, 3, 1.5f, 0.4f, new int[] { 7, 0, 6, 0, 8, 0, 5, 0, 9, 0, 0 }, 0f, 1.02f));
        stageDatas.Add(new StageData(8, 20, 3, 2, 2f, 0.3f, new int[] { 5, 0, 6, 0, 1, 0, 4, 0, 0, 7, 0, 8, 0, 0 }, 0f));
        stageDatas.Add(new StageData(9, 20, 4, 1, 2f, 0.3f, new int[] { 7, 0, 0, 8, 0, 0, 7, 0, 0, 8, 0, 0, 7, 0, 0, 8, 0, 0, 7, 0, 0, 8, 0, 0 }, 0f, 0.925f));
        stageDatas.Add(new StageData(10, 20, 3, 2, 1.5f, 0.4f, new int[] { 5, 0, 6, 0, 5, 0, 6, 0, 0, 7, 0, 8, 0, 7, 0, 8, 0, 11, 11, 0, 0 }, 1f));
        stageDatas.Add(new StageData(11, 20, 2, 3, 2f, 0.25f, new int[] { 5, 0, 5, 0, 6, 0, 6, 0, 7, 0, 7, 0, 8, 0, 8, 0, 8, 0, 8, 0, 7, 0, 7, 0, 6, 0, 6, 0, 5, 0, 5, 0, 0 }, 0.1f, 1.1f));
        stageDatas.Add(new StageData(12, 20, 4, 3, 2f, 0.25f, new int[] { 11, 0, 7, 0, 0, 11, 0, 8, 0, 0, 11, 0, 6, 0, 11, 0, 5, 0, 10, 0, 9, 0, 7, 0, 8, 0, 0 }, 0));
        stageDatas.Add(new StageData(13, 20, 3, 3, 1f, 0.5f, new int[] { 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 10, 0, 0 }, 0));
        stageDatas.Add(new StageData(14, 20, 2, 2, 1f, 0.5f, new int[] { 5, 0, 7, 0, 6, 0, 8, 0, 5, 0, 7, 0, 6, 0, 8, 0, 5, 0, 7, 0, 6, 0, 8, 0, 5, 0, 7, 0, 6, 0, 8, 0, 1, 0, 2, 0, 3, 0, 4, 0, 0 }, 0.1f, 1.2f));
        stageDatas.Add(new StageData(15, 20, 3, 3, 1.5f, 0.3f, new int[] { 7, 0, 7, 0, 7, 0, 8, 0, 8, 0, 8, 0, 7, 0, 7, 0, 7, 0, 8, 0, 8, 0, 8, 0, 9, 0, 0 }, 0.4f, 1.067f));
        stageDatas.Add(new StageData(16, 20, 3, 4, 2f, 0.25f, new int[] { 10, 0, 0, 11, 11, 0, 9, 0, 11, 11, 0, 10, 0, 11, 11, 0, 10, 0, 11, 11, 0, 9, 0, 11, 11, 0, 10, 0, 0 }, 0));
        stageDatas.Add(new StageData(17, 20, 4, 4, 2f, 0.25f, new int[] { 5, 0, 9, 0, 10, 0, 6, 0, 10, 0, 9, 0, 8, 0, 10, 0, 9, 0, 7, 0, 9, 0, 10, 0, 5, 0, 10, 0, 9, 0, 6, 0, 9, 0, 10, 0, 8, 0, 0 }, 1.5f));
        stageDatas.Add(new StageData(18, 20, 2, 2, 1f, 0.5f, new int[] { 7, 0, 8, 0, 7, 0, 8, 0, 11, 11, 11, 11, 0, 8, 0, 7, 0, 8, 0, 7, 0, 0, 11, 11, 11, 11, 0, 0 }, 1.4f));
        stageDatas.Add(new StageData(19, 20, 2, 1, 1f, 0.5f, new int[] { 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 0, 0 }, 0));
        stageDatas.Add(new StageData(20, 20, 4, 4, 1f, 0.5f, new int[] { 11, 11, 11, 11, 0, 7, 0, 8, 0, 0, 11, 11, 11, 11, 0, 8, 0, 7, 0, 0, 11, 11, 11, 11, 0, 5, 0, 6, 0, 0, 11, 11, 11, 11, 0, 5, 0, 6, 0, 0, 11, 11, 11, 11, 11, 11, 11, 11, 11, 0, 9, 0, 0 }, 2f, 0.87f));
        stageDatas.Add(new StageData(21, 20, 2, 2, 0.5f, 0.8f, new int[] { 5, 0, 7, 0, 6, 0, 8, 0, 5, 0, 7, 0, 6, 0, 8, 0, 5, 0, 7, 0, 6, 0, 8, 0, 0, 11, 11, 11, 11, 1, 0, 0 }, 0, 0.8f));
        stageDatas.Add(new StageData(22, 15, 1, 3, 2f, 0.2f, new int[] { 11, 11, 11, 11, 11, 11, 11, 11, 0, 102, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 0, 0 }, 0f, 0.86f));
        stageDatas.Add(new StageData(23, 10, 2, 3, 1f, 0.4f, new int[] { 5, 0, 6, 0, 5, 0, 6, 0, 5, 0, 6, 0, 5, 0, 6, 101, 5, 0, 6, 0, 5, 0, 6, 101, 7, 0, 0 }, 1f));
        stageDatas.Add(new StageData(24, 15, 2, 1, 2f, 0.2f, new int[] { 11, 11, 11, 11, 11, 11, 11, 11, 102, 0, 11, 11, 11, 11, 11, 11, 11, 11, 0, 0, 101, 11, 11, 11, 11, 0, 0 }, 0.05f));
        stageDatas.Add(new StageData(25, 20, 5, 5, 2f, 0.2f, new int[] { 9, 0, 10, 0, 9, 0, 10, 0, 11, 10, 0, 9, 0, 10, 0, 9, 0, 102, 7, 0, 8, 0, 7, 0, 8, 0, 0 }, 0.2f));
        stageDatas.Add(new StageData(26, 20, 4, 4, 1f, 0.4f, new int[] { 11, 11, 11, 0, 102, 7, 101, 11, 11, 11, 11, 102, 8, 0, 101, 11, 11, 11, 0, 102, 5, 101, 11, 11, 11, 11, 102, 6, 0, 0 }, 2.1f, 0.92f));
        stageDatas.Add(new StageData(27, 1, 2, 2, 3.5f, 0.1f, new int[] { 1, 4, 0, 101, 11, 0, 0 }, 0.9f, 1f));
        stageDatas.Add(new StageData(28, 25, 3, 3, 4f, 0.1f, new int[] { 5, 0, 6, 102, 11, 11, 102, 1, 0, 1, 0, 11, 0, 5, 0, 5, 0, 6, 0, 11, 11, 11, 7, 0, 8, 0, 7, 0, 8, 0, 11, 0, 0 }, 2f, 0.99f));
        stageDatas.Add(new StageData(29, 20, 3, 3, 2.5f, 0.15f, new int[] { 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 0, 0 }, 1f, 0.96f));
        stageDatas.Add(new StageData(30, 20, 3, 3, 2f, 0.2f, new int[] { 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 0, 0 }, 1.5f));
        stageDatas.Add(new StageData(31, 20, 3, 3, 2f, 0.15f, new int[] { 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 0, 0 }, 1.8f));
        stageDatas.Add(new StageData(32, 20, 3, 3, 1f, 0.3f, new int[] { 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 0, 0 }, 2f, 0.94f));
        stageDatas.Add(new StageData(33, 20, 3, 3, 2f, 0.15f, new int[] { 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 0, 0 }, 1.3f, 0.9f));
        stageDatas.Add(new StageData(34, 20, 3, 3, 2f, 0.15f, new int[] { 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 0, 0 }, 1.2f, 0.9f));
        stageDatas.Add(new StageData(35, 25, 3, 3, 2f, 0.15f, new int[] { 11, 11, 0, 102, 11, 11, 11, 11, 11, 11, 11, 0, 101, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 0, 102, 11, 11, 11, 11, 11, 11, 11, 11, 0, 101, 11, 11, 11, 11, 11, 11, 0, 0 }, 0.5f));
        stageDatas.Add(new StageData(36, 15, 3, 3, 1.5f, 0.2f, new int[] { 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 0, 0 }, 0f, 1.13f));
        stageDatas.Add(new StageData(37, 15, 3, 3, 1f, 0.3f, new int[] { 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 0, 0 }, 3.7f, 0.88f));
        stageDatas.Add(new StageData(38, 15, 3, 3, 2f, 0.15f, new int[] { 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 0, 0 }, 0.2f, 0.86f));
        stageDatas.Add(new StageData(39, 15, 3, 3, 2.5f, 0.1f, new int[] { 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 0, 0 }, 1f, 0.96f));
        stageDatas.Add(new StageData(40, 15, 3, 3, 2f, 0.15f, new int[] { 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 0, 0 }, 1.2f, 0.92f));
        stageDatas.Add(new StageData(41, 15, 3, 3, 1f, 0.3f, new int[] { 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 0, 0 }, 1f, 0.82f));
        stageDatas.Add(new StageData(42, 15, 3, 3, 2f, 0.15f, new int[] { 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 0, 0 }, 11.4f, 0.85f));
        stageDatas.Add(new StageData(43, 15, 3, 3, 3f, 0.1f, new int[] { 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 0, 0 }, 1.7f,0.94f));
        stageDatas.Add(new StageData(44, 15, 3, 3, 0.5f, 0.4f, new int[] { 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 0, 0 }, 0.1f, 0.89f));
        stageDatas.Add(new StageData(45, 15, 3, 3, 2f, 0.1f, new int[] { 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 0, 0 }, 0.9f, 0.86f));
        stageDatas.Add(new StageData(46, 10, 3, 3, 0.5f, 0.4f, new int[] { 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 0, 0 }, 7.7f, 0.87f));
        stageDatas.Add(new StageData(47, 10, 3, 3, 2f, 0.1f, new int[] { 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 0, 0 }, 4f, 0.92f));
        stageDatas.Add(new StageData(48, 10, 3, 3, 2f, 0.1f, new int[] { 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 0, 0 }, 3.2f, 0.86f));
        stageDatas.Add(new StageData(49, 10, 3, 3, 2f, 0.1f, new int[] { 9, 10, 9, 10, 9, 10, 9, 10, 9, 10, 9, 10, 9, 10, 9, 10, 9, 10, 9, 10, 9, 10, 9, 10, 9, 10, 9, 10, 9, 10, 9, 10, 9, 10, 9, 10, 9, 10, 9, 10, 9, 10, 9, 10, 9, 10, 9, 10, 9, 10, 9, 10, 9, 10, 9, 10, 9, 10, 9, 10, 9, 10, 9, 10, 9, 10, 9, 10, 9, 10, 9, 10, 9, 10, 9, 10, 9, 10, 9, 10, 0, 0 }, 2.8f, 0.92f));
        stageDatas.Add(new StageData(50, 10, 7, 7, 0.5f, 0.4f, new int[] { 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 0, 0 }, 0));
        DontDestroyOnLoad(gameObject);
    }
}

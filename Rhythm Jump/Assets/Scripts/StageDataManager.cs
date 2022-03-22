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
        stageDatas = new List<StageData>();
        stageDatas.Add(null);
        stageDatas.Add(new StageData(1, 30, 2, 3, 2, 0.3f, new int[] { 1, 0, 3, 0, 5, 0, 6, 0, 7, 0, 8, 0, 0 }, 0f));
        stageDatas.Add(new StageData(2, 30, 3, 2, 2, 0.3f, new int[] { 5, 0, 6, 0, 5, 0, 6, 0, 0, 7, 0, 8, 0, 7, 0, 0, 5, 0, 0, 7, 0, 0, 6, 0, 0, 8, 0, 0 }, 0f));
        stageDatas.Add(new StageData(3, 30, 3, 3, 2, 0.3f, new int[] { 7, 0, 6, 0, 8, 0, 5, 0, 7, 0, 6, 0, 8, 0, 5, 0, 9, 0 }, 0f));
        stageDatas.Add(new StageData(4, 30, 2, 3, 2, 0.3f, new int[] { 5, 0, 6, 0, 5, 0, 6, 0, 5, 0, 6, 0, 5, 0, 6, 0, 0 }, 0));
        stageDatas.Add(new StageData(5, 30, 3, 2, 2, 0.3f, new int[] { 5, 0, 6, 0, 5, 0, 6, 0, 7, 0, 8, 0, 7, 0, 8, 0, 11, 11, 0 }, 0));
        stageDatas.Add(new StageData(6, 30, 5, 1, 2, 0.3f, new int[] { 1, 0, 0, 2, 0, 0, 1, 0, 0, 2, 0, 0, 1, 0, 0, 2, 0, 0, 1, 0, 0, 2, 0, 0 }, 0));
        stageDatas.Add(new StageData(7, 30, 3, 3, 2, 0.3f, new int[] { 7, 0, 7, 0, 7, 0, 8, 0, 8, 0, 8, 0, 7, 0, 7, 0, 7, 0, 8, 0, 8, 0, 8, 0, 9, 0 }, 0));
        stageDatas.Add(new StageData(8, 20, 3, 3, 2, 0.3f, new int[] { 9, 0, 10, 0, 9, 0, 10, 0, 11, 0, 11, 0, 11, 0, 11, 0, 11, 0, 11, 0, 11, 0, 11, 0, 9, 0, 10, 0, 9, 0, 10, 0, 0 }, 0));
        stageDatas.Add(new StageData(9, 15, 3, 3, 2, 0.3f, new int[] { 11, 0, 11, 0, 11, 0, 11, 0, 11, 0, 11, 0, 11, 0, 11, 0, 11, 0, 11, 0, 10, 0, 0 }, 0));
        stageDatas.Add(new StageData(10, 25, 2, 2, 1, 0.5f, new int[] { 5, 0, 7, 0, 6, 0, 8, 0, 5, 0, 7, 0, 6, 0, 8, 0, 5, 0, 7, 0, 6, 0, 8, 0, 5, 0, 7, 0, 6, 0, 8, 0, 1, 0, 2, 0, 3, 0, 4, 0, 0 }, 0));
        stageDatas.Add(new StageData(11, 10, 2, 3, 2, 0.25f, new int[] { 5, 0, 5, 0, 6, 0, 6, 0, 7, 0, 7, 0, 8, 0, 8, 0, 8, 0, 8, 0, 7, 0, 7, 0, 6, 0, 6, 0, 5, 0, 5, 0, 0 }, 0));
        stageDatas.Add(new StageData(12, 10, 3, 2, 2, 0.25f, new int[] { 5, 0, 5, 0, 6, 0, 6, 0, 5, 0, 5, 0, 6, 0, 6, 0, 5, 0, 5, 0, 5, 0, 6, 0, 6, 0, 6, 0, 7, 0, 0 }, 0));
        stageDatas.Add(new StageData(13, 10, 2, 4, 2, 0.25f, new int[] { 11,0,5, 0, 6, 0, 5, 0, 6, 0 }, 0));
        stageDatas.Add(new StageData(14, 10, 4, 2, 2, 0.25f, new int[] { 5, 0, 6, 0, 5, 0, 6, 0 }, 0));
        stageDatas.Add(new StageData(15, 10, 3, 3, 2, 0.25f, new int[] { 5, 0, 6, 0, 5, 0, 6, 0 }, 0));
        stageDatas.Add(new StageData(16, 10, 3, 4, 1.5f, 0.25f, new int[] { 5, 0, 6, 0, 5, 0, 6, 0 }, 0));
        stageDatas.Add(new StageData(17, 10, 4, 3, 2, 0.25f, new int[] { 5, 0, 6, 0, 5, 0, 6, 0 }, 0));
        stageDatas.Add(new StageData(18, 10, 3, 3, 2, 0.25f, new int[] { 5, 0, 6, 0, 5, 0, 6, 0 }, 0));
        stageDatas.Add(new StageData(19, 10, 4, 4, 2, 0.25f, new int[] { 5, 0, 6, 0, 5, 0, 6, 0 }, 0));
        stageDatas.Add(new StageData(20, 10, 2, 1, 2, 0.25f, new int[] { 11, 0, 11, 0, 11, 0, 11, 0, 11, 0, 11, 0, 11, 0, 11, 0, 11, 0, 11, 0, 11, 0, 11, 0, 11, 0, 11, 0, 11, 0, 11, 0, 11, 0, 11, 0, 11, 0, 11, 0, 11, 0, 11, 0, 11, 0, 11, 0, 11, 0, 11, 0 }, 0));
        stageDatas.Add(new StageData(21, 10, 3, 3, 2, 0.2f, new int[] { 5, 0, 6, 0, 5, 0, 6, 0 }, 0));
        stageDatas.Add(new StageData(22, 10, 3, 3, 2, 0.2f, new int[] { 5, 0, 6, 0, 5, 0, 6, 0 }, 0));
        stageDatas.Add(new StageData(23, 10, 3, 3, 2, 0.2f, new int[] { 5, 0, 6, 0, 5, 0, 6, 0 }, 0));
        stageDatas.Add(new StageData(24, 10, 3, 3, 2, 0.2f, new int[] { 5, 0, 6, 0, 5, 0, 6, 0 }, 0));
        stageDatas.Add(new StageData(25, 10, 3, 3, 2, 0.2f, new int[] { 5, 0, 6, 0, 5, 0, 6, 0 }, 0));
        stageDatas.Add(new StageData(26, 10, 3, 3, 2, 0.2f, new int[] { 5, 0, 6, 0, 5, 0, 6, 0 }, 0));
        stageDatas.Add(new StageData(27, 10, 3, 3, 2, 0.2f, new int[] { 5, 0, 6, 0, 5, 0, 6, 0 }, 0));
        stageDatas.Add(new StageData(28, 10, 3, 3, 2, 0.2f, new int[] { 5, 0, 6, 0, 5, 0, 6, 0 }, 0));
        stageDatas.Add(new StageData(29, 10, 3, 3, 2, 0.2f, new int[] { 5, 0, 6, 0, 5, 0, 6, 0 }, 0));
        stageDatas.Add(new StageData(30, 10, 3, 3, 2, 0.2f, new int[] { 5, 0, 6, 0, 5, 0, 6, 0 }, 0));
        stageDatas.Add(new StageData(31, 10, 3, 3, 2, 0.15f, new int[] { 5, 0, 6, 0, 5, 0, 6, 0 }, 0));
        stageDatas.Add(new StageData(32, 10, 3, 3, 2, 0.15f, new int[] { 5, 0, 6, 0, 5, 0, 6, 0 }, 0));
        stageDatas.Add(new StageData(33, 10, 3, 3, 2, 0.15f, new int[] { 5, 0, 6, 0, 5, 0, 6, 0 }, 0));
        stageDatas.Add(new StageData(34, 10, 3, 3, 2, 0.15f, new int[] { 5, 0, 6, 0, 5, 0, 6, 0 }, 0));
        stageDatas.Add(new StageData(35, 10, 3, 3, 2, 0.15f, new int[] { 5, 0, 6, 0, 5, 0, 6, 0 }, 0));
        stageDatas.Add(new StageData(36, 10, 3, 3, 2, 0.15f, new int[] { 5, 0, 6, 0, 5, 0, 6, 0 }, 0));
        stageDatas.Add(new StageData(37, 10, 3, 3, 2, 0.15f, new int[] { 5, 0, 6, 0, 5, 0, 6, 0 }, 0));
        stageDatas.Add(new StageData(38, 10, 3, 3, 2, 0.15f, new int[] { 5, 0, 6, 0, 5, 0, 6, 0 }, 0));
        stageDatas.Add(new StageData(39, 10, 3, 3, 2, 0.15f, new int[] { 5, 0, 6, 0, 5, 0, 6, 0 }, 0));
        stageDatas.Add(new StageData(40, 10, 3, 3, 2, 0.15f, new int[] { 5, 0, 6, 0, 5, 0, 6, 0 }, 0));
        stageDatas.Add(new StageData(41, 10, 3, 3, 2, 0.1f, new int[] { 5, 0, 6, 0, 5, 0, 6, 0 }, 0));
        stageDatas.Add(new StageData(42, 10, 3, 3, 2, 0.1f, new int[] { 5, 0, 6, 0, 5, 0, 6, 0 }, 0));
        stageDatas.Add(new StageData(43, 10, 3, 3, 2, 0.1f, new int[] { 5, 0, 6, 0, 5, 0, 6, 0 }, 0));
        stageDatas.Add(new StageData(44, 10, 3, 3, 2, 0.1f, new int[] { 5, 0, 6, 0, 5, 0, 6, 0 }, 0));
        stageDatas.Add(new StageData(45, 10, 3, 3, 2, 0.1f, new int[] { 5, 0, 6, 0, 5, 0, 6, 0 }, 0));
        stageDatas.Add(new StageData(46, 10, 3, 3, 2, 0.1f, new int[] { 5, 0, 6, 0, 5, 0, 6, 0 }, 0));
        stageDatas.Add(new StageData(47, 10, 3, 3, 2, 0.1f, new int[] { 5, 0, 6, 0, 5, 0, 6, 0 }, 0));
        stageDatas.Add(new StageData(48, 10, 3, 3, 2, 0.1f, new int[] { 5, 0, 6, 0, 5, 0, 6, 0 }, 0));
        stageDatas.Add(new StageData(49, 10, 3, 3, 2, 0.1f, new int[] { 5, 0 }, 0));
        stageDatas.Add(new StageData(50, 10, 7, 7, 2, 0.1f, new int[] { 5, 0 }, 0));
        DontDestroyOnLoad(gameObject);
    }
}

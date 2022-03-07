using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageDataManager : MonoBehaviour
{
    public List<StageData> stageDatas;

    void Awake()
    {
        stageDatas = new List<StageData>();
        stageDatas.Add(null);
        stageDatas.Add(new StageData(1, 10, 2, 3, 2, 0.2f, new int[] { 1, 2, 3, 4 }));
        stageDatas.Add(new StageData(2, 10, 3, 3, 2, 0.2f, new int[] { 5, 0, 6, 0, 5, 0, 6, 0 }));
        stageDatas.Add(new StageData(3, 10, 3, 3, 2, 0.2f, new int[] { 5, 0, 6, 0, 5, 0, 6, 0 }));
        stageDatas.Add(new StageData(4, 10, 3, 3, 2, 0.2f, new int[] { 5, 0, 6, 0, 5, 0, 6, 0 }));
        stageDatas.Add(new StageData(5, 10, 3, 3, 2, 0.2f, new int[] { 5, 0, 6, 0, 5, 0, 6, 0 }));
        stageDatas.Add(new StageData(6, 10, 3, 3, 2, 0.2f, new int[] { 5, 0, 6, 0, 5, 0, 6, 0 }));
        stageDatas.Add(new StageData(7, 10, 3, 3, 2, 0.2f, new int[] { 5, 0, 6, 0, 5, 0, 6, 0 }));
        stageDatas.Add(new StageData(8, 10, 3, 3, 2, 0.2f, new int[] { 5, 0, 6, 0, 5, 0, 6, 0 }));
        stageDatas.Add(new StageData(9, 10, 3, 3, 2, 0.2f, new int[] { 5, 0, 6, 0, 5, 0, 6, 0 }));
        stageDatas.Add(new StageData(10, 10, 3, 3, 2, 0.2f, new int[] { 5, 0, 6, 0, 5, 0, 6, 0 }));
        stageDatas.Add(new StageData(11, 10, 3, 3, 2, 0.2f, new int[] { 5, 0, 6, 0, 5, 0, 6, 0 }));
        stageDatas.Add(new StageData(12, 10, 3, 3, 2, 0.2f, new int[] { 5, 0, 6, 0, 5, 0, 6, 0 }));
        stageDatas.Add(new StageData(13, 10, 3, 3, 2, 0.2f, new int[] { 5, 0, 6, 0, 5, 0, 6, 0 }));
        stageDatas.Add(new StageData(14, 10, 3, 3, 2, 0.2f, new int[] { 5, 0, 6, 0, 5, 0, 6, 0 }));
        stageDatas.Add(new StageData(15, 10, 3, 3, 2, 0.2f, new int[] { 5, 0, 6, 0, 5, 0, 6, 0 }));
        stageDatas.Add(new StageData(16, 10, 3, 3, 2, 0.2f, new int[] { 5, 0, 6, 0, 5, 0, 6, 0 }));
        stageDatas.Add(new StageData(17, 10, 3, 3, 2, 0.2f, new int[] { 5, 0, 6, 0, 5, 0, 6, 0 }));
        stageDatas.Add(new StageData(18, 10, 3, 3, 2, 0.2f, new int[] { 5, 0, 6, 0, 5, 0, 6, 0 }));
        stageDatas.Add(new StageData(19, 10, 3, 3, 2, 0.2f, new int[] { 5, 0, 6, 0, 5, 0, 6, 0 }));
        stageDatas.Add(new StageData(20, 10, 3, 3, 2, 0.2f, new int[] { 5, 0, 6, 0, 5, 0, 6, 0 }));
        stageDatas.Add(new StageData(21, 10, 3, 3, 2, 0.2f, new int[] { 5, 0, 6, 0, 5, 0, 6, 0 }));
        stageDatas.Add(new StageData(22, 10, 3, 3, 2, 0.2f, new int[] { 5, 0, 6, 0, 5, 0, 6, 0 }));
        stageDatas.Add(new StageData(23, 10, 3, 3, 2, 0.2f, new int[] { 5, 0, 6, 0, 5, 0, 6, 0 }));
        stageDatas.Add(new StageData(24, 10, 3, 3, 2, 0.2f, new int[] { 5, 0, 6, 0, 5, 0, 6, 0 }));
        stageDatas.Add(new StageData(25, 10, 3, 3, 2, 0.2f, new int[] { 5, 0, 6, 0, 5, 0, 6, 0 }));
        stageDatas.Add(new StageData(26, 10, 3, 3, 2, 0.2f, new int[] { 5, 0, 6, 0, 5, 0, 6, 0 }));
        stageDatas.Add(new StageData(27, 10, 3, 3, 2, 0.2f, new int[] { 5, 0, 6, 0, 5, 0, 6, 0 }));
        stageDatas.Add(new StageData(28, 10, 3, 3, 2, 0.2f, new int[] { 5, 0, 6, 0, 5, 0, 6, 0 }));
        stageDatas.Add(new StageData(29, 10, 3, 3, 2, 0.2f, new int[] { 5, 0, 6, 0, 5, 0, 6, 0 }));
        stageDatas.Add(new StageData(30, 10, 3, 3, 2, 0.2f, new int[] { 5, 0, 6, 0, 5, 0, 6, 0 }));
        stageDatas.Add(new StageData(31, 10, 3, 3, 2, 0.2f, new int[] { 5, 0, 6, 0, 5, 0, 6, 0 }));
        stageDatas.Add(new StageData(32, 10, 3, 3, 2, 0.2f, new int[] { 5, 0, 6, 0, 5, 0, 6, 0 }));
        stageDatas.Add(new StageData(33, 10, 3, 3, 2, 0.2f, new int[] { 5, 0, 6, 0, 5, 0, 6, 0 }));
        stageDatas.Add(new StageData(34, 10, 3, 3, 2, 0.2f, new int[] { 5, 0, 6, 0, 5, 0, 6, 0 }));
        stageDatas.Add(new StageData(35, 10, 3, 3, 2, 0.2f, new int[] { 5, 0, 6, 0, 5, 0, 6, 0 }));
        stageDatas.Add(new StageData(36, 10, 3, 3, 2, 0.2f, new int[] { 5, 0, 6, 0, 5, 0, 6, 0 }));
        stageDatas.Add(new StageData(37, 10, 3, 3, 2, 0.2f, new int[] { 5, 0, 6, 0, 5, 0, 6, 0 }));
        stageDatas.Add(new StageData(38, 10, 3, 3, 2, 0.2f, new int[] { 5, 0, 6, 0, 5, 0, 6, 0 }));
        stageDatas.Add(new StageData(39, 10, 3, 3, 2, 0.2f, new int[] { 5, 0, 6, 0, 5, 0, 6, 0 }));
        stageDatas.Add(new StageData(40, 10, 3, 3, 2, 0.2f, new int[] { 5, 0, 6, 0, 5, 0, 6, 0 }));
        stageDatas.Add(new StageData(41, 10, 3, 3, 2, 0.2f, new int[] { 5, 0, 6, 0, 5, 0, 6, 0 }));
        stageDatas.Add(new StageData(42, 10, 3, 3, 2, 0.2f, new int[] { 5, 0, 6, 0, 5, 0, 6, 0 }));
        stageDatas.Add(new StageData(43, 10, 3, 3, 2, 0.2f, new int[] { 5, 0, 6, 0, 5, 0, 6, 0 }));
        stageDatas.Add(new StageData(44, 10, 3, 3, 2, 0.2f, new int[] { 5, 0, 6, 0, 5, 0, 6, 0 }));
        stageDatas.Add(new StageData(45, 10, 3, 3, 2, 0.2f, new int[] { 5, 0, 6, 0, 5, 0, 6, 0 }));
        stageDatas.Add(new StageData(46, 10, 3, 3, 2, 0.2f, new int[] { 5, 0, 6, 0, 5, 0, 6, 0 }));
        stageDatas.Add(new StageData(47, 10, 3, 3, 2, 0.2f, new int[] { 5, 0, 6, 0, 5, 0, 6, 0 }));
        stageDatas.Add(new StageData(48, 10, 3, 3, 2, 0.2f, new int[] { 5, 0, 6, 0, 5, 0, 6, 0 }));
        stageDatas.Add(new StageData(49, 10, 3, 3, 2, 0.2f, new int[] { 5, 0 }));
        stageDatas.Add(new StageData(50, 10, 7, 7, 1, 0.1f, new int[] { 5, 0 }));
        DontDestroyOnLoad(gameObject);
    }
}

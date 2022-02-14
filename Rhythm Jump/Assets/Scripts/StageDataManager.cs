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
        DontDestroyOnLoad(gameObject);
    }
}

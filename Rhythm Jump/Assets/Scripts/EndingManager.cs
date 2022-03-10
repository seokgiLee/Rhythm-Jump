using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class EndingManager : MonoBehaviour
{
    public SFXManager sfxManager;
    public BGMManager bgmManager;

    public AudioMixer audioMixer;
    public float sfx;
    public float bgm;

    public GameObject player;
    public GameObject floor;
    public GameObject[] creditTexts;
    public Text endText;

    void Awake()
    {
        player.transform.DOMoveY(7, 45f);
        floor.transform.DOMoveY(7, 45f);
        for (int i = 0; i < creditTexts.Length; i++)
        {
            creditTexts[i].transform.DOMoveY(-700, 20f).SetDelay(5f + 7f * i);
        }
        endText.DOColor(new Color(1, 1, 1, 1), 3f).SetDelay(30f);
        Invoke("EndGame", 35f);
    }

    void EndGame()
    {
        LoadingCanvasManager.Instance.ChangeScene("Main Scene");
    }
}

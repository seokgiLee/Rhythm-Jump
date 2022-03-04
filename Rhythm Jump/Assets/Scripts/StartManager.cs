using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class StartManager : MonoBehaviour
{
    public SFXManager sfxManager;
    public BGMManager bgmManager;
    public CamaerManager camaerManager;
    public Button startButton;
    public Text startButtonText;

    public AudioMixer audioMixer;
    public float sfx;
    public float bgm;

    void Awake()
    {
        sfx = PlayerPrefs.GetFloat("SFX");
        bgm = PlayerPrefs.GetFloat("BGM");
        audioMixer.SetFloat("SFX", sfx);
        audioMixer.SetFloat("BGM", bgm);
        FadeOut();
    }

    void Update()
    {
        
    }

    public void FadeOut()
    {
        startButtonText.transform.DOScale(1.1f, 0.1f);
        Invoke("FadeIn", 0.1f);
    }
    public void FadeIn()
    {
        startButtonText.transform.DOScale(1f, 0.1f);
        Invoke("FadeOut", 0.5f);
    }


    public void StartButton()
    {
        sfxManager.PlaySound(8);
        if (PlayerPrefs.HasKey("Max Stage"))
        {
            LoadingCanvasManager.Instance.ChangeScene("Main Scene");
        }
        else
        {
            LoadingCanvasManager.Instance.ChangeScene("Tutorial Scene");
        }

        startButton.interactable = false;
        camaerManager.Zoom(0);
    }
}

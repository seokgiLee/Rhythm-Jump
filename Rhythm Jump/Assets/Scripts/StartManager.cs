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

    void Start()
    {
        if (PlayerPrefs.HasKey("SFX"))
        {
            sfx = PlayerPrefs.GetFloat("SFX");
            audioMixer.SetFloat("SFX", sfx);
        }
        else
        {
            audioMixer.SetFloat("SFX", -20);
        }

        if(PlayerPrefs.HasKey("BGM"))
        {
            bgm = PlayerPrefs.GetFloat("BGM");
            audioMixer.SetFloat("BGM", bgm);
        }
        else
        {
            audioMixer.SetFloat("BGM", -20);
        }

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
        bgmManager.StopSound();
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

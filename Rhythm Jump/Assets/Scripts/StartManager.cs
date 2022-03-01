using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartManager : MonoBehaviour
{
    public SFXManager sfxManager;
    public BGMManager bgmManager;
    public CamaerManager camaerManager;
    public Button startButton;
    public Text startButtonText;

    void Awake()
    {
        FadeOut();
    }

    void Update()
    {
        
    }

    public void FadeOut()
    {
        startButtonText.DOFade(0, 1);
        Invoke("FadeIn", 1f);
    }
    public void FadeIn()
    {
        startButtonText.DOFade(1, 2);
        Invoke("FadeOut", 2f);
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

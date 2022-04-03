using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class StartManager : MonoBehaviour
{
    public AudioSource audioSource;
    public GameObject pause;
    public float backButtonTime; // 스마트폰 뒤로가기 버튼용 타이머
    public bool backButton; // 스마트폰 뒤로가기 버튼 클릭가능 여부

    public SFXManager sfxManager;
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

        audioSource.Play();
        FadeOut();

        Application.targetFrameRate = 60;
    }

    void Update()
    {
        // 스마트폰 뒤로가기버튼
        if (!backButton)
        {
            backButtonTime += Time.deltaTime;
            if (backButtonTime > 0.5f) // 0.5초마다 클릭가능
            {
                backButton = true;
                backButtonTime = 0;
            }
        }

        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Escape) && backButton) // 뒤로가기
            {
                if (pause.activeSelf) // 일시정지창이 켜져있으면 끄기
                {
                    ContinueButton();
                }
                else // 일시정지창이 꺼져있으면 일시정지
                {
                    PauseButton();
                }
                backButton = false;
            }
        }
    }

    public void PauseButton() // 일시정지 버튼
    {
        sfxManager.PlaySound(0);
        Time.timeScale = 0;
        pause.SetActive(true);
    }

    public void ContinueButton() // 계속하기 버튼
    {
        sfxManager.PlaySound(0);
        Time.timeScale = 1;
        pause.SetActive(false);
    }

    public void ExitButton() // 나가기 버튼
    {
        sfxManager.PlaySound(0);
        Application.Quit();
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

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
    public float backButtonTime; // ����Ʈ�� �ڷΰ��� ��ư�� Ÿ�̸�
    public bool backButton; // ����Ʈ�� �ڷΰ��� ��ư Ŭ������ ����

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
        // ����Ʈ�� �ڷΰ����ư
        if (!backButton)
        {
            backButtonTime += Time.deltaTime;
            if (backButtonTime > 0.5f) // 0.5�ʸ��� Ŭ������
            {
                backButton = true;
                backButtonTime = 0;
            }
        }

        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Escape) && backButton) // �ڷΰ���
            {
                if (pause.activeSelf) // �Ͻ�����â�� ���������� ����
                {
                    ContinueButton();
                }
                else // �Ͻ�����â�� ���������� �Ͻ�����
                {
                    PauseButton();
                }
                backButton = false;
            }
        }
    }

    public void PauseButton() // �Ͻ����� ��ư
    {
        sfxManager.PlaySound(0);
        Time.timeScale = 0;
        pause.SetActive(true);
    }

    public void ContinueButton() // ����ϱ� ��ư
    {
        sfxManager.PlaySound(0);
        Time.timeScale = 1;
        pause.SetActive(false);
    }

    public void ExitButton() // ������ ��ư
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public AudioClip[] bgmAudios;
    AudioSource audioSource;
    public int curStageNum;

    void Awake()
    {
        if (!PlayerPrefs.HasKey("BGM"))
        {
            PlayerPrefs.SetFloat("BGM", -20);
        }

        audioSource = GetComponent<AudioSource>();
        DontDestroyOnLoad(gameObject);
    }

    public void PlaySound(int i)
    {
        audioSource.clip = bgmAudios[i];
        audioSource.Play();
    }

    public void StopSound()
    {
        audioSource.Stop();
    }
}

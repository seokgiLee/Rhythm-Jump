using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class EndingManager : MonoBehaviour
{
    public SFXManager sfxManager;
    public BGMManager bgmManager;
    public CamaerManager camaerManager;
    public Button startButton;
    public Text startButtonText;

    public AudioMixer audioMixer;
    public float sfx;
    public float bgm;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public AudioClip[] sfxAudios;
    AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        DontDestroyOnLoad(gameObject);
    }

    public void PlaySound(int i)
    {
        // 0: 클릭
        // 1: 이동
        // 2: 박자
        // 3: 폭발
        // 4: 에러
        // 5: 승리
        // 6: 패배
        // 7: 씬 시작
        // 8: 씬 종료
        // 9: 박자힌트
        // 10: 대화창 표시
        // 11: 카운트다운

        audioSource.PlayOneShot(sfxAudios[i]);
    }
}

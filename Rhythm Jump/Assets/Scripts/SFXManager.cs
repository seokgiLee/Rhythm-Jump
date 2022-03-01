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
        // 0: Ŭ��
        // 1: �̵�
        // 2: ����
        // 3: ����
        // 4: ����
        // 5: �¸�
        // 6: �й�
        // 7: �� ����
        // 8: �� ����
        // 9: ������Ʈ
        // 10: ��ȭâ ǥ��
        // 11: ī��Ʈ�ٿ�

        audioSource.PlayOneShot(sfxAudios[i]);
    }
}

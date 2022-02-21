using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class LoadingCanvasManager : MonoBehaviour
{
    public GameObject Loading;
    public CanvasGroup fade;
    float fadeDuration = 0.5f; //�����Ǵ� �ð�

    public static LoadingCanvasManager Instance
    {
        get
        {
            return instance;
        }
    }
    private static LoadingCanvasManager instance;

    void Awake()
    {
        if (instance != null)
        {
            DestroyImmediate(this.gameObject);
            return;
        }
        instance = this;

        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded; // �̺�Ʈ�� �߰�
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // �̺�Ʈ���� ����*
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        fade.DOFade(0, fadeDuration)
        .OnStart(() => {
            Loading.SetActive(false);
        })
        .OnComplete(() => {
            fade.blocksRaycasts = false;
        });
    }

    public void ChangeScene(string sceneName)
    {
        fade.DOFade(1, fadeDuration)
        .OnStart(() =>
        {
            fade.blocksRaycasts = true; //�Ʒ� ����ĳ��Ʈ ����
        })
        .OnComplete(() =>
        {
            StartCoroutine("LoadScene", sceneName); //�ε�ȭ�� ����, �� �ε� ����
        });
    }

    IEnumerator LoadScene(string sceneName)
    {
        Loading.SetActive(true); //�ε� ȭ���� ���

        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);
        async.allowSceneActivation = false; //�ۼ�Ʈ �����̿�

        float past_time = 0;
        float percentage = 0;

        while (!(async.isDone))
        {
            yield return null;

            past_time += Time.deltaTime;

            if (percentage >= 90)
            {
                percentage = Mathf.Lerp(percentage, 100, past_time);

                if (percentage == 100)
                {
                    async.allowSceneActivation = true; //�� ��ȯ �غ� �Ϸ�
                }
            }
            else
            {
                percentage = Mathf.Lerp(percentage, async.progress * 100f, past_time);
                if (percentage >= 90) past_time = 0;
            }
        }
    }
}

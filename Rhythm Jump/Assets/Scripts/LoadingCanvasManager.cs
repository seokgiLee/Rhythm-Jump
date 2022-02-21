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
    float fadeDuration = 0.5f; //암전되는 시간

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
        SceneManager.sceneLoaded += OnSceneLoaded; // 이벤트에 추가
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // 이벤트에서 제거*
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
            fade.blocksRaycasts = true; //아래 레이캐스트 막기
        })
        .OnComplete(() =>
        {
            StartCoroutine("LoadScene", sceneName); //로딩화면 띄우며, 씬 로드 시작
        });
    }

    IEnumerator LoadScene(string sceneName)
    {
        Loading.SetActive(true); //로딩 화면을 띄움

        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);
        async.allowSceneActivation = false; //퍼센트 딜레이용

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
                    async.allowSceneActivation = true; //씬 전환 준비 완료
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

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum SCENE { None, Load, Main, Play }

public enum FadeInOutTypes
{
    Fade_Out_Default,
    Fade_In_Default,
    Fade_Out_Up,
    Fade_In_Up,
    None,
}


public class SceneTransitionManager : MonoSingleton<SceneTransitionManager>
{
    public float fadeDuration = 1f; // 페이드 인/아웃 지속 시간

    bool isTransition = false;

    // 다른 코드에서 씬 전환 요청을 받는 메서드
    public void TransitionToScene(SCENE closeScene, SCENE openScene, FadeInOutTypes fadeOutType, FadeInOutTypes fadeInType)
    {
        if (!isTransition)
            StartCoroutine(Transition(closeScene, openScene, fadeOutType, fadeInType));
    }

    // 페이드 아웃, 씬 전환, 페이드 인을 처리하는 코루틴
    private IEnumerator Transition(SCENE closeScene, SCENE openScene, FadeInOutTypes fadeOutType, FadeInOutTypes fadeInType)
    {
        isTransition = true;
        yield return CallFadeEffect(fadeOutType);

        AsyncOperation asyncLoad;

        // 씬 전환
        if (closeScene != SCENE.None)
        {
            asyncLoad = SceneManager.UnloadSceneAsync(closeScene.ToString() + "Scene");
            yield return new WaitUntil(() => asyncLoad.isDone);
        }
        if (openScene != SCENE.None)
        {
            asyncLoad = SceneManager.LoadSceneAsync(openScene.ToString() + "Scene", LoadSceneMode.Additive);
            yield return new WaitUntil(() => asyncLoad.isDone);
        }

        yield return CallFadeEffect(fadeInType);

        isTransition = false;
    }

    public IEnumerator CallFadeEffect(FadeInOutTypes type)
    {
        switch (type)
        {
            case FadeInOutTypes.Fade_Out_Default:
                yield return StartCoroutine(FadeOutDefault());
                break;
            case FadeInOutTypes.Fade_Out_Up:
                yield return StartCoroutine(FadeOutUp());
                break;
            case FadeInOutTypes.Fade_In_Default:
                yield return StartCoroutine(FadeInDefault());
                break;
            case FadeInOutTypes.Fade_In_Up:
                yield return StartCoroutine(FadeInUp());
                break;
        }
    }

    public CanvasGroup defaultBlack;

    private IEnumerator FadeOutDefault()
    {
        // 페이드 아웃
        while (defaultBlack.alpha < 1f)
        {
            defaultBlack.alpha += Time.deltaTime / fadeDuration;
            yield return null;
        }
    }
    private IEnumerator FadeInDefault()
    {
        // 페이드 인
        while (defaultBlack.alpha > 0f)
        {
            defaultBlack.alpha -= Time.deltaTime / fadeDuration;
            yield return null;
        }
    }
    public RectTransform fadeOutUpRT;
    public RectTransform fadeInUpRT;

    private IEnumerator FadeOutUp()
    {
        float time = 0;
        fadeOutUpRT.anchoredPosition = new Vector2(0, -1080);
        fadeOutUpRT.gameObject.SetActive(true);

        while (time < fadeDuration)
        {
            fadeOutUpRT.anchoredPosition = new Vector2(0, Mathf.Lerp(-1080, 1080, time / fadeDuration));

            time += Time.deltaTime;
            yield return null;
        }

        fadeOutUpRT.anchoredPosition = new Vector2(0, 1080);
        defaultBlack.alpha = 1;
        fadeOutUpRT.gameObject.SetActive(false);
    }
    private IEnumerator FadeInUp()
    {
        float time = 0;
        fadeInUpRT.anchoredPosition = new Vector2(0, -1080);
        defaultBlack.alpha = 0;
        fadeInUpRT.gameObject.SetActive(true);

        while (time < fadeDuration)
        {
            fadeInUpRT.anchoredPosition = new Vector2(0, Mathf.Lerp(-1080, 1080, time / fadeDuration));

            time += Time.deltaTime;
            yield return null;
        }

        fadeInUpRT.anchoredPosition = new Vector2(0, 1080);
        fadeInUpRT.gameObject.SetActive(false);
    }
}

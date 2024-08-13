using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SCENE { None, Load, Main, Play, Option }
public enum FadeTypes { Default, Up, None, Quick }
public enum IO { In, Out }

public class SceneTransitionManager : MonoSingleton<SceneTransitionManager>
{
    public float fadeDuration = 1f; // 페이드 인/아웃 지속 시간

    bool isTransition = false;
    public bool IsTransition { get { return isTransition; } }

    public bool IsSceneLoaded(string name)
    {
        Scene scene = SceneManager.GetSceneByName(name);
        return scene.isLoaded;
    }

    // 다른 코드에서 씬 전환 요청을 받는 메서드
    public void TransitionToScene(SCENE closeScene, SCENE openScene, FadeTypes fadeOutType, FadeTypes fadeInType)
    {
        if (!isTransition)
            StartCoroutine(Transition(closeScene, openScene, fadeOutType, fadeInType));
    }

    // 페이드 아웃, 씬 전환, 페이드 인을 처리하는 코루틴
    private IEnumerator Transition(SCENE closeScene, SCENE openScene, FadeTypes fadeOutType, FadeTypes fadeInType)
    {
        isTransition = true;
        yield return CallFadeEffect(fadeOutType, IO.Out);

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

        yield return CallFadeEffect(fadeInType, IO.In);

        isTransition = false;
    }

    public void TransitionToNextStage()
    {
        StartCoroutine(TransitionToNextStageCo());
    }

    private IEnumerator TransitionToNextStageCo()
    {
        StageManager.stageIndex++;
        yield return Transition(SCENE.Play, SCENE.Play, FadeTypes.None, FadeTypes.None);
    }

    public void RestartStage()
    {
        StartCoroutine(RestartStageCo());
    }

    private IEnumerator RestartStageCo()
    {
        yield return Transition(SCENE.Play, SCENE.Play, FadeTypes.None, FadeTypes.None);
    }


    #region FadeInOut

    public CanvasGroup defaultBlack;
    public RectTransform fadeOutUpRT;
    public RectTransform fadeInUpRT;

    public IEnumerator CallFadeEffect(FadeTypes type, IO io)
    {
        if (type == FadeTypes.None)
            yield break;

        string CoName = "Fade" + io.ToString() + type.ToString();

        yield return StartCoroutine(CoName);
    }

    private IEnumerator FadeOutDefault()
    {
        // 페이드 아웃
        while (defaultBlack.alpha < 1f)
        {
            defaultBlack.alpha += Time.deltaTime / fadeDuration;
            yield return PlayTime.ScaledNull;
        }
    }
    private IEnumerator FadeInDefault()
    {
        // 페이드 인
        while (defaultBlack.alpha > 0f)
        {
            defaultBlack.alpha -= Time.deltaTime / fadeDuration;
            yield return PlayTime.ScaledNull;
        }
    }

    private IEnumerator FadeOutQuick()
    {
        defaultBlack.alpha = 1;
        yield break;
    }
    private IEnumerator FadeInQuick()
    {
        defaultBlack.alpha = 0;
        yield break;
    }

    private IEnumerator FadeOutUp()
    {
        float time = 0;
        fadeOutUpRT.anchoredPosition = new Vector2(0, -1080);
        fadeOutUpRT.gameObject.SetActive(true);

        while (time < fadeDuration)
        {
            fadeOutUpRT.anchoredPosition = new Vector2(0, Mathf.Lerp(-1080, 1080, time / fadeDuration));

            time += Time.deltaTime;
            yield return PlayTime.ScaledNull;
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
            yield return PlayTime.ScaledNull;
        }

        fadeInUpRT.anchoredPosition = new Vector2(0, 1080);
        fadeInUpRT.gameObject.SetActive(false);
    }

    #endregion
}

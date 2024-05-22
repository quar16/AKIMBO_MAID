using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SCENE { None, Load, Main, Play }

public class SceneTransitionManager : MonoSingleton<SceneTransitionManager>
{
    public float fadeDuration = 1f; // 페이드 인/아웃 지속 시간
    public CanvasGroup fadeCanvasGroup; // 페이드 인/아웃을 위한 캔버스 그룹

    private Coroutine transitionCoroutine; // 코루틴 참조

    // 다른 코드에서 씬 전환 요청을 받는 메서드
    public void TransitionToScene(SCENE closeScene, SCENE openScene)
    {
        if (transitionCoroutine != null)
        {
            StopCoroutine(transitionCoroutine);
        }
        transitionCoroutine = StartCoroutine(Transition(closeScene, openScene));
    }

    // 페이드 아웃, 씬 전환, 페이드 인을 처리하는 코루틴
    private IEnumerator Transition(SCENE closeScene, SCENE openScene)
    {
        // 페이드 아웃
        fadeCanvasGroup.alpha = 0f;
        while (fadeCanvasGroup.alpha < 1f)
        {
            fadeCanvasGroup.alpha += Time.deltaTime / fadeDuration;
            yield return null;
        }

        // 전환 전 상태를 전달하는 글로벌 메시지
        Debug.Log("전환 전 상태를 전달합니다.");

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

        // 페이드 인
        while (fadeCanvasGroup.alpha > 0f)
        {
            fadeCanvasGroup.alpha -= Time.deltaTime / fadeDuration;
            yield return null;
        }

        // 전환 후 상태를 전달하는 글로벌 메시지
        Debug.Log("전환 후 상태를 전달합니다.");
    }
}

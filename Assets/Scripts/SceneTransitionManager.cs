using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SCENE { None, Load, Main, Play }

public class SceneTransitionManager : MonoSingleton<SceneTransitionManager>
{
    public float fadeDuration = 1f; // ���̵� ��/�ƿ� ���� �ð�
    public CanvasGroup fadeCanvasGroup; // ���̵� ��/�ƿ��� ���� ĵ���� �׷�

    private Coroutine transitionCoroutine; // �ڷ�ƾ ����

    // �ٸ� �ڵ忡�� �� ��ȯ ��û�� �޴� �޼���
    public void TransitionToScene(SCENE closeScene, SCENE openScene)
    {
        if (transitionCoroutine != null)
        {
            StopCoroutine(transitionCoroutine);
        }
        transitionCoroutine = StartCoroutine(Transition(closeScene, openScene));
    }

    // ���̵� �ƿ�, �� ��ȯ, ���̵� ���� ó���ϴ� �ڷ�ƾ
    private IEnumerator Transition(SCENE closeScene, SCENE openScene)
    {
        // ���̵� �ƿ�
        fadeCanvasGroup.alpha = 0f;
        while (fadeCanvasGroup.alpha < 1f)
        {
            fadeCanvasGroup.alpha += Time.deltaTime / fadeDuration;
            yield return null;
        }

        // ��ȯ �� ���¸� �����ϴ� �۷ι� �޽���
        Debug.Log("��ȯ �� ���¸� �����մϴ�.");

        AsyncOperation asyncLoad;

        // �� ��ȯ
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

        // ���̵� ��
        while (fadeCanvasGroup.alpha > 0f)
        {
            fadeCanvasGroup.alpha -= Time.deltaTime / fadeDuration;
            yield return null;
        }

        // ��ȯ �� ���¸� �����ϴ� �۷ι� �޽���
        Debug.Log("��ȯ �� ���¸� �����մϴ�.");
    }
}

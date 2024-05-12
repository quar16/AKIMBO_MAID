using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SCENE { Load, Main, Play }

public class SceneTransitionManager : MonoSingleton<SceneTransitionManager>
{
    public float fadeDuration = 1f; // ���̵� ��/�ƿ� ���� �ð�
    public CanvasGroup fadeCanvasGroup; // ���̵� ��/�ƿ��� ���� ĵ���� �׷�

    private Coroutine transitionCoroutine; // �ڷ�ƾ ����

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(fadeCanvasGroup.transform.parent.gameObject);

    }

    // �ٸ� �ڵ忡�� �� ��ȯ ��û�� �޴� �޼���
    public void TransitionToScene(string sceneName)
    {
        SCENE sceneNameE;

        if (System.Enum.TryParse(sceneName, out sceneNameE))
            transitionCoroutine = StartCoroutine(Transition(sceneNameE));
        else
            Debug.LogWarning("��ȿ���� ���� string �� ȣ�� : " + sceneName);

    }
    // �ٸ� �ڵ忡�� �� ��ȯ ��û�� �޴� �޼���
    public void TransitionToScene(SCENE sceneName)
    {
        if (transitionCoroutine != null)
        {
            StopCoroutine(transitionCoroutine);
        }
        transitionCoroutine = StartCoroutine(Transition(sceneName));
    }

    // ���̵� �ƿ�, �� ��ȯ, ���̵� ���� ó���ϴ� �ڷ�ƾ
    private IEnumerator Transition(SCENE sceneName)
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

        // �� ��ȯ
        SceneManager.LoadScene(sceneName.ToString() + "Scene");

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

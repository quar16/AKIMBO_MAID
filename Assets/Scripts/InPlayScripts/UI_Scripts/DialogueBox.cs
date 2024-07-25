using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DialogueBox : MonoBehaviour
{
    public Text uiText;
    public float fadeDuration = 0.5f;
    public float moveDistance = 100f;
    public float textDelay = 0.1f;

    private RectTransform rectTransform;
    private Vector2 originalPosition;
    private CanvasGroup canvasGroup;

    public IEnumerator ShowDialogue(string dialogue)
    {
        rectTransform = GetComponent<RectTransform>();
        // RectTransform의 원래 위치를 저장
        originalPosition = rectTransform.anchoredPosition;
        // CanvasGroup 컴포넌트 추가 (투명도 제어용)
        canvasGroup = GetComponent<CanvasGroup>();


        yield return StartCoroutine(FadeInAndMoveUp());
        yield return StartCoroutine(TypeText(dialogue));
    }

    public void HideBox()
    {
        StartCoroutine(FadeOutAndMoveDown());
    }

    private IEnumerator FadeInAndMoveUp()
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / fadeDuration;

            // 투명도 조절
            canvasGroup.alpha = Mathf.Lerp(0, 1, t);
            // 위치 조절
            rectTransform.anchoredPosition = Vector2.Lerp(originalPosition - new Vector2(0, moveDistance), originalPosition, t);

            yield return PlayTime.ScaledNull;
        }

        canvasGroup.alpha = 1;
        rectTransform.anchoredPosition = originalPosition;
    }

    private IEnumerator TypeText(string dialogue)
    {
        string currentText;

        for (int i = 0; i < dialogue.Length; i++)
        {
            currentText = dialogue.Substring(0, i + 1);
            uiText.text = currentText;
            yield return PlayTime.ScaledWaitForSeconds(textDelay);
        }
    }

    private IEnumerator FadeOutAndMoveDown()
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / fadeDuration;

            // 투명도 조절
            canvasGroup.alpha = Mathf.Lerp(1, 0, t);
            // 위치 조절
            rectTransform.anchoredPosition = Vector2.Lerp(originalPosition, originalPosition - new Vector2(0, moveDistance), t);

            yield return PlayTime.ScaledNull;
        }

        Destroy(gameObject);
    }
}

using System.Collections;
using UnityEngine;

public class RoundUI : MonoBehaviour
{
    public RectTransform fullRound;
    public RectTransform shell;
    public float gravity = 1;
    public float yPower = 1;
    public float xPower = 1;
    public float rotPower = 1;

    public void AnimateShellEjectionAsyncall()
    {
        StartCoroutine(AnimateShellEjectionAsync());
    }

    IEnumerator AnimateShellEjectionAsync()
    {
        fullRound.gameObject.SetActive(false);

        shell.gameObject.SetActive(true);

        float ySpeed = Random.Range(1, 4f);
        float xSpeed = Random.Range(1, 3f) - ySpeed;
        float rotSpeed = Random.Range(100, 200) * (Random.Range(0, 2) * 2 - 1) * rotPower;


        while (shell.anchoredPosition.y > -1500)
        {
            shell.anchoredPosition += new Vector2(xSpeed * xPower, ySpeed * yPower);
            ySpeed -= Time.deltaTime * gravity;
            shell.eulerAngles += Vector3.forward * rotSpeed * PlayTime.Scale;
            yield return PlayTime.ScaledNull;
        }
        shell.gameObject.SetActive(false);
        shell.anchoredPosition = Vector2.zero;
    }

    public void AnimateReloadAsyncall()
    {
        StartCoroutine(AnimateReloadAsync());
    }

    IEnumerator AnimateReloadAsync()
    {
        fullRound.anchoredPosition = Vector2.up * 20;
        fullRound.gameObject.SetActive(true);

        float time = Time.time;
        float duration = 0.3f;
        Vector2 startPos = fullRound.anchoredPosition;

        while (time + duration > Time.time)
        {
            float t = (Time.time - time) / duration;

            fullRound.anchoredPosition = Vector2.Lerp(startPos, Vector2.zero, Mathf.Sin(t * Mathf.PI * 0.5f));
            yield return PlayTime.ScaledNull;
        }
        fullRound.anchoredPosition = Vector2.zero;
    }
}

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
            shell.eulerAngles += Vector3.forward * rotSpeed;
            yield return null;
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

        for (int i = 0; i < 20; i++)
        {
            fullRound.anchoredPosition = Vector2.Lerp(fullRound.anchoredPosition, Vector2.zero, 0.1f);
            yield return null;
        }
        fullRound.anchoredPosition = Vector2.zero;
    }
}

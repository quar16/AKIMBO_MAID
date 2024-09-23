using JetBrains.Annotations;
using System.Collections;
using UnityEngine;

public class TempScript2 : MonoBehaviour
{
    public float timeScale = 1;
    public float FPS = 60;

    public int targetFPS = 60;

    public Transform box;

    private void Start()
    {
        StartCoroutine(ccon());
        StartCoroutine(Circle());

        Application.targetFrameRate = 60;
        Time.timeScale = 1;
    }

    private void Update()
    {
        timeScale = Time.timeScale;
        FPS = 1 / Time.deltaTime;
    }

    IEnumerator Circle()
    {
        while (true)
        {
            box.eulerAngles += new Vector3(0, 0, 0.5f) * Time.timeScale * Time.deltaTime * 60;

            yield return null;
        }
    }

    IEnumerator ccon()
    {
        while (true)
        {
            yield return null;

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                targetFPS = Mathf.Clamp(targetFPS - 10, 1, 300);
                Application.targetFrameRate = targetFPS;
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                targetFPS = Mathf.Clamp(targetFPS + 10, 1, 300);
                Application.targetFrameRate = targetFPS;
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                Time.timeScale = Mathf.Clamp01(Time.timeScale - 0.1f);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                Time.timeScale = Mathf.Clamp01(Time.timeScale + 0.1f);
            }
        }
    }
}

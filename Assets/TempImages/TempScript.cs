using System.Collections;
using UnityEngine;

public class TempScript : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(co1());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Time.timeScale = 1.5f - Time.timeScale;
        }
    }

    IEnumerator co1()
    {
        while (true)
        {
            yield return wfs;
            Debug.Log("asdad");
        }
    }

    WaitForSeconds wfs = new WaitForSeconds(1);
}


public class WaitForSecondsCustom : CustomYieldInstruction
{
    private readonly float waitTime;
    private readonly float startTime;

    public WaitForSecondsCustom(float time)
    {
        waitTime = time;
        startTime = Time.time;
    }

    public override bool keepWaiting
    {
        get
        {
            return Time.time < startTime + waitTime;
        }
    }
}
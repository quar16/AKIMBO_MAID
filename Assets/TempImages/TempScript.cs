using System.Collections;
using UnityEngine;

public class TempScript : MonoBehaviour
{
    public Camera camera;
    public Animator animator;
    public GameObject blind;

    public bool dead = false;

    public void Start()
    {
        StartCoroutine(co());
    }

    IEnumerator co()
    {
        yield return new WaitUntil(() => dead);
        animator.Play("Player_Dead");

        float time = Time.time;

        while (time + 1 > Time.time)
        {
            float t = Time.time - time;

            camera.orthographicSize = 5 * Mathf.Cos(t * 0.5f * Mathf.PI);

            yield return null;
        }
        blind.SetActive(true);
    }
}

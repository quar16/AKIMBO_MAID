using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScopeEffect : MonoBehaviour
{
    Transform target;
    bool isInInsight = true;

    public void Init(DamageableObject _target)
    {
        target = _target.transform;
        isInInsight = true;
        StartCoroutine(DisplayScopeEffect());
    }

    public void Hide()
    {
        isInInsight = false;
    }

    public void Update()
    {
        if (target == null)
        {
            Debug.Log("적은 죽었지만 스코프 살아있음");
            Destroy(gameObject);
            return;
        }

        transform.position = target.position + new Vector3(0.5f, 0.5f, 0);
    }

    IEnumerator DisplayScopeEffect()
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();

        Color color = renderer.color;


        float time = Time.time;
        float duration = 0.06f;
        while (time + duration > Time.time)
        {
            float t = (Time.time - time) / duration;
            transform.localScale = Vector3.one * (2 - t);
            color.a = t * 0.5f;
            renderer.color = color;
            yield return PlayTime.ScaledNull;
        }

        yield return new WaitWhile(() => isInInsight);

        time = Time.time;
        while (time + duration > Time.time)
        {
            float t = 1 - (Time.time - time) / duration;
            transform.localScale = Vector3.one * (2 - t);
            color.a = t * 0.5f;
            renderer.color = color;
            yield return PlayTime.ScaledNull;
        }

        Destroy(gameObject);
    }
}

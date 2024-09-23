using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScopeEffect : MonoBehaviour
{
    Entity target;
    bool isInInsight = true;

    public void Init(Entity _target)
    {
        target = _target;
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
        if (GameManager.Instance.gameMode == GameMode.NARRATIVE)
        {
            Destroy(gameObject);
            return;
        }

        transform.position = target.CenterPoint;
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
            color.a = t;
            renderer.color = color;
            yield return PlayTime.ScaledNull;
        }

        transform.localScale = Vector3.one;
        color.a = 1;
        renderer.color = color;

        yield return new WaitWhile(() => isInInsight);

        time = Time.time;
        while (time + duration > Time.time)
        {
            float t = 1 - (Time.time - time) / duration;
            transform.localScale = Vector3.one * (2 - t);
            color.a = t;
            renderer.color = color;
            yield return PlayTime.ScaledNull;
        }

        Destroy(gameObject);
    }
}

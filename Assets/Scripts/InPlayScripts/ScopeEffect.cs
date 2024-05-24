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

        transform.position = target.position;
    }

    IEnumerator DisplayScopeEffect()
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();

        Color color = renderer.color;

        for (int i = 0; i <= 5; i++)
        {
            transform.localScale = Vector3.one * (2 - 0.2f * i);
            color.a = i * 0.1f;
            renderer.color = color;
            yield return null;
        }

        yield return new WaitWhile(() => isInInsight);

        for (int i = 5; i >= 0; i--)
        {
            transform.localScale = Vector3.one * (2 - 0.2f * i);
            color.a = i * 0.1f;
            renderer.color = color;
            yield return null;
        }

        Destroy(gameObject);
    }
}

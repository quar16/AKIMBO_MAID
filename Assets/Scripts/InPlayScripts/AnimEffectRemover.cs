using System.Collections;
using UnityEngine;

public class AnimEffectRemover : MonoBehaviour
{
    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();

        if (animator == null)
            Destroy(this);

        StartCoroutine(DestoryAnim());
    }

    IEnumerator DestoryAnim()
    {
        yield return new WaitWhile(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1);

        Destroy(gameObject);
    }
}

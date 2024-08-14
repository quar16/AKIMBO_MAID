using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaProjectile : MonoBehaviour
{
    public int damage;
    bool canAttack = false;
    protected Vector2 size;

    public void Activate(float duration)
    {
        gameObject.SetActive(true);
        StartCoroutine(Processing(duration));
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        canAttack = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (canAttack && collision.gameObject.CompareTag("Player"))
        {
            canAttack = false;
            PlayerHealthManager.Instance.ChangeHealth(-damage);
        }
    }

    protected virtual IEnumerator Processing(float duration)
    {
        yield break;
    }

    public virtual void SetSize(Vector2 _size)
    {
        size = _size;
        GetComponent<BoxCollider2D>().size = size;
    }
}

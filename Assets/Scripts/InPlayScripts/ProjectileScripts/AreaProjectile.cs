using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AreaProjectile : MonoBehaviour
{
    public int damage;
    bool canAttack = false;
    protected Vector2 size;

    protected Transform carrier;


    public void Activate(float duration, bool isLeft)
    {
        if (carrier == null)
        {
            carrier = new GameObject(gameObject.name + "Carrier").transform;
            SceneManager.MoveGameObjectToScene(carrier.gameObject, gameObject.scene);
        }

        carrier.transform.position = transform.position;
        carrier.transform.localScale = new Vector3(isLeft ? 1 : -1, 1, 1);
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

    private void OnTriggerStay2D(Collider2D collision)
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

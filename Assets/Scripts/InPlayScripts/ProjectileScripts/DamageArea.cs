using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageArea : MonoBehaviour
{
    public int damage;
    public float interval;

    BoxCollider2D coll;
    float nextDamageTime = 0;

    public void Init(Vector3 position, Vector2 size)
    {
        transform.localPosition = position;
        
        if (coll == null)
            coll = GetComponent<BoxCollider2D>();

        coll.size = size;
    }

    public void On()
    {
        gameObject.SetActive(true);
    }

    public void Off()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Time.time < nextDamageTime) return;

        if (collision.CompareTag("Player"))
        {
            nextDamageTime = Time.time + interval;
            PlayerHealthManager.Instance.ChangeHealth(-damage);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            nextDamageTime = 0;
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaProjectile : MonoBehaviour
{
    public int damage;
    bool canAttack = false;
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
}

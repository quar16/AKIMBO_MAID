using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : DamageableObject
{
    PlayerDetection playerDetection;

    private void Start()
    {
        playerDetection = GetComponentInChildren<PlayerDetection>();

        if (playerDetection != null)
            playerDetection.enemy = this;

        nowHP = maxHP;
    }

    public override void OnZeroHP()
    {
        Destroy(gameObject);
        OppositionEntityManager.Instance.DespawnEntity(this);
    }

    public virtual void Attack()
    {

    }

    public virtual void DetectPlayer()
    {

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : DamageableObject
{
    PlayerDetection playerDetection;

    private void Start()
    {
        playerDetection = GetComponentInChildren<PlayerDetection>();
        playerDetection.enemy = this;
        nowHP = maxHP;
    }

    public override void OnZeroHP()
    {
        Destroy(gameObject);
    }

    public virtual void Attack()
    {

    }

    public virtual void DetectPlayer()
    {

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : DamageableObject
{
    private void Start()
    {
        nowHP = maxHP;
    }

    public override void OnZeroHP()
    {
        Destroy(gameObject);
    }
}

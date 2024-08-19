using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    private void Start()
    {
        nowHP = maxHP;
    }

    public override void OnZeroHP()
    {
        OppositionEntityManager.Instance.DespawnEntity(this);
    }
}

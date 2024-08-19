using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaProjectile_AroundAtk : AreaProjectile
{
    public Animator slashEffect;

    protected override IEnumerator Processing(float duration)
    {
        this.InstantiateEffect(slashEffect, transform.position, Quaternion.identity, 0.5f, carrier);

        yield return PlayTime.ScaledWaitForSeconds(duration);
    }
}
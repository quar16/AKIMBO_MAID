using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaProjectile_JumpAtk : AreaProjectile
{
    public Animator slashEffect;

    protected override IEnumerator Processing(float duration)
    {
        Vector3 pos = transform.position;
        this.InstantiateEffect(slashEffect, pos, Quaternion.identity, 2 / 3f, carrier);

        yield return PlayTime.ScaledWaitForSeconds(duration);
    }
}
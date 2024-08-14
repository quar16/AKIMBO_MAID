using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaProjectile_TrackingAtk : AreaProjectile
{
    public Animator slashEffect;

    protected override IEnumerator Processing(float duration)
    {
        Vector3 pos = transform.position;

        float sTime = Time.time;

        this.InstantiateEffect(slashEffect, pos, Quaternion.Euler(0, 0, 150));
        yield return PlayTime.ScaledWaitForSeconds(duration * 0.3f);
        this.InstantiateEffect(slashEffect, pos, Quaternion.Euler(0, 0, 30));
        yield return PlayTime.ScaledWaitForSeconds(duration * 0.3f);
        this.InstantiateEffect(slashEffect, pos, Quaternion.Euler(0, 0, 90));

        yield return new WaitWhile(() => sTime + duration > Time.time);
    }
}
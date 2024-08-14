using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaProjectile_TeleportAtk : AreaProjectile
{
    public Animator slashEffect;

    protected override IEnumerator Processing(float duration)
    {
        Vector3 pos = transform.position;

        float sTime = Time.time;

        this.InstantiateEffect(slashEffect, pos, Quaternion.Euler(-30.7f, 75.7f, 0), 1.4f);
        yield return PlayTime.ScaledWaitForSeconds(duration * 0.1f);
        this.InstantiateEffect(slashEffect, pos, Quaternion.Euler(103.38f, 0, 0), 1.4f);
        yield return PlayTime.ScaledWaitForSeconds(duration * 0.1f);
        this.InstantiateEffect(slashEffect, pos, Quaternion.Euler(32.2f, 75.7f, 0), 1.4f);

        yield return new WaitWhile(() => sTime + duration > Time.time);
    }
}

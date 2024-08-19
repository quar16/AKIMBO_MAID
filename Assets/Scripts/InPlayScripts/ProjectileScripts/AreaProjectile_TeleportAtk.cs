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

        InstantiateEffect(Quaternion.Euler(-30.7f, 75.7f, 0));
        yield return PlayTime.ScaledWaitForSeconds(duration * 0.1f);
        InstantiateEffect(Quaternion.Euler(103.38f, 0, 0));
        yield return PlayTime.ScaledWaitForSeconds(duration * 0.1f);
        InstantiateEffect(Quaternion.Euler(32.2f, 75.7f, 0));

        yield return new WaitWhile(() => sTime + duration > Time.time);
    }

    void InstantiateEffect(Quaternion euler)
    {
        this.InstantiateEffect(slashEffect, transform.position, euler, 1.4f, carrier);
    }
}

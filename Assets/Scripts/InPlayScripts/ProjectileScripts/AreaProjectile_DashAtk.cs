using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaProjectile_DashAtk : AreaProjectile
{
    public Animator slashEffect;

    protected override IEnumerator Processing(float duration)
    {
        float sTime = Time.time;
        Vector3 leftEnd = transform.position - new Vector3(size.x * 0.5f, 0, 0);
        Vector3 rightEnd = transform.position + new Vector3(size.x * 0.5f, 0, 0);

        while (sTime + duration > Time.time)
        {
            float t = (Time.time - sTime) / duration;

            Vector3 currentPosition = Vector3.Lerp(leftEnd, rightEnd, t);
            currentPosition += 0.5f * size.y * new Vector3(Random.Range(-1f, 1), Random.Range(-1f, 1), 0);

            float randomAngle = Random.Range(-30f, 30f); // 각도 범위 조정 가능

            this.InstantiateEffect(slashEffect, currentPosition, Quaternion.Euler(0, 0, randomAngle), 1, carrier);

            yield return PlayTime.ScaledNull;
        }

        yield break;
    }
}
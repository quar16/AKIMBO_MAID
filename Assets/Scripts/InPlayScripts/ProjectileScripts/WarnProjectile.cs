using System.Collections;
using UnityEngine;

public class WarnProjectile : MonoBehaviour
{
    public GameObject warn;
    public GameObject projectile;

    public void Init(Vector3 pos, bool isLeft)
    {
        transform.position = pos + Vector3.up * 0.5f;

        if (!isLeft)
            transform.localScale = new Vector3(-1, 1, 1);
    }
    public void Warn(float warnTime)
    {
        StartCoroutine(Warning(warnTime));
    }

    public void Shoot(float projectileDuration)
    {
        StartCoroutine(Shooting(projectileDuration));
    }

    protected virtual IEnumerator Warning(float warnTime)
    {
        warn.SetActive(true);
        yield return PlayTime.ScaledWaitForSeconds(warnTime);
        warn.SetActive(false);
    }

    protected virtual IEnumerator Shooting(float projectileDuration)
    {
        warn.SetActive(false);
        projectile.SetActive(true);
        yield return PlayTime.ScaledWaitForSeconds(projectileDuration);
        projectile.SetActive(false);
    }
}

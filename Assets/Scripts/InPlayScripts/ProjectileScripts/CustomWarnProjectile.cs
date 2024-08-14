using System.Collections;
using UnityEngine;

public class CustomWarnProjectile : MonoBehaviour
{
    public SpriteRenderer warnBox;
    public AreaProjectile projectile;

    public void Init(Vector3 pos, bool isLeft, Vector2 size)
    {
        transform.position = pos + Vector3.up * 0.5f;

        warnBox.size = size;
        projectile.SetSize(size);

        if (!isLeft)
            transform.localScale = new Vector3(-1, 1, 1);
    }

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
        warnBox.gameObject.SetActive(true);

        float sTime = Time.time;

        while (sTime + warnTime > Time.time)
        {
            float t = (Time.time - sTime) / warnTime;

            warnBox.color = new Color(1, 1, 1, 0.5f + t * 0.5f);

            yield return PlayTime.ScaledNull;
        }

        warnBox.gameObject.SetActive(false);
    }

    protected virtual IEnumerator Shooting(float projectileDuration)
    {
        warnBox.gameObject.SetActive(false);
        projectile.Activate(projectileDuration);
        yield return PlayTime.ScaledWaitForSeconds(projectileDuration);
        projectile.Deactivate();
    }
}

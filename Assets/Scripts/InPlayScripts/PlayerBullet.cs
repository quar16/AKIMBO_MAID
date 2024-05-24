using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public LineRenderer lineRenderer;
    PlayerGun playerGun;

    float shake = 0.3f;
    float speed = 1;

    public float duration = 0.1f;
    public Color startColor = Color.yellow;
    public Color endColor = Color.black;

    Vector3 startPoint;
    Vector3 endPoint;

    public void Init(PlayerGun _playerGun)
    {
        playerGun = _playerGun;
        lineRenderer = GetComponent<LineRenderer>();
        Deactivate();
    }

    public void Fire(Vector2 firePoint, Vector2 fireDirection, DamageableObject target)
    {
        DrawLine(firePoint, fireDirection);
        ApplyDamage(target);
        StartCoroutine(FadeLine());
    }

    public void DrawLine(Vector3 _startPoint, Vector3 _endPoint)
    {
        Vector3 shakePoint = new Vector3(Random.Range(-1, 1f), Random.Range(-1, 1f), 0) * shake;

        startPoint = _startPoint + shakePoint;
        endPoint = _endPoint + shakePoint;

        lineRenderer.SetPosition(0, startPoint);
        lineRenderer.SetPosition(1, endPoint);
    }

    private IEnumerator FadeLine()
    {
        float elapsedTime = 0f;
        float initialWidth = 0.1f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            lineRenderer.startWidth = Mathf.Lerp(initialWidth, 0, t);
            lineRenderer.endWidth = Mathf.Lerp(initialWidth, 0, t);

            lineRenderer.startColor = Color.Lerp(startColor, endColor, t);
            lineRenderer.endColor = Color.Lerp(startColor, endColor, t);



            Vector3 direction = endPoint - startPoint;
            direction.Normalize();
            float totalDistance = Vector3.Distance(startPoint, endPoint);
            float clampedDistance = Mathf.Min(speed, totalDistance);
            startPoint = startPoint + direction * clampedDistance;
            lineRenderer.SetPosition(0, startPoint);

            yield return null;
        }

        lineRenderer.startWidth = 0;
        lineRenderer.endWidth = 0;

        lineRenderer.startColor = endColor;
        lineRenderer.endColor = endColor;

        Deactivate();
    }

    //히트판정 전반을 계산하도록 수정
    private void ApplyDamage(DamageableObject target)
    {
        if (target != null)
            target.GetDamage(1);
    }

    private void Deactivate()
    {
        playerGun.EnqueueBullet(this);
        gameObject.SetActive(false);
    }
}
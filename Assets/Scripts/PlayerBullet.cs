using System.Collections;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    // 총알 이동 속도
    public float speed = 10f;
    Renderer rendererC;


    bool IsVisible { get { return rendererC.isVisible; } }

    PlayerGun playerGun;

    public void Init(PlayerGun _playerGun)
    {
        playerGun = _playerGun;
        rendererC = GetComponent<Renderer>();
        Deactivate();
    }

    public void Fire(Vector2 firePoint, Vector2 fireDirection)
    {
        StartCoroutine(Firing(firePoint, fireDirection));
    }

    IEnumerator Firing(Vector2 firePoint, Vector2 fireDirection)
    {
        transform.position = firePoint;
        rendererC.enabled = true;
        yield return null;

        while (IsVisible)
        {
            transform.Translate(fireDirection * speed * Time.deltaTime);
            yield return null;
        }

        Deactivate();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(""))
        {
            GameObject hitTarget = collision.gameObject;
            //hitTarget.~ 대미지 받는 처리
            //총알 이펙트 보이기 등

            rendererC.enabled = false;
        }
    }

    private void Deactivate()
    {
        playerGun.EnqueueBullet(this);
        gameObject.SetActive(false);
    }
}
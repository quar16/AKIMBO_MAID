using System.Collections;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    // �Ѿ� �̵� �ӵ�
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
            //hitTarget.~ ����� �޴� ó��
            //�Ѿ� ����Ʈ ���̱� ��

            rendererC.enabled = false;
        }
    }

    private void Deactivate()
    {
        playerGun.EnqueueBullet(this);
        gameObject.SetActive(false);
    }
}
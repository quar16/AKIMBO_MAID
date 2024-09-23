using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField]
    int damage = 1;
    [SerializeField]
    float speed = 10f;

    private Renderer _renderer;

    Vector3 direction;

    int unrenderCount = 0;

    void Start()
    {
        _renderer = GetComponent<Renderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealthManager.Instance.ChangeHealth(-damage);

            gameObject.SetActive(false);
        }
    }

    public void Initialize(Vector3 position, Vector3 _direction)
    {
        transform.position = position;
        direction = _direction;
    }

    public void Update()
    {
        transform.position += direction * speed * 0.01f * PlayTime.Scale;

        if (!_renderer.isVisible)
            unrenderCount++;
        if (unrenderCount >= 10)
            Deactive();
    }

    public void Deactive()
    {
        unrenderCount = 0;
        ProjectilePool.Instance.ReturnProjectile(gameObject);
        gameObject.SetActive(false);
    }
}

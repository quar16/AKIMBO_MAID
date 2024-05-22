using UnityEngine;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Collections;

public enum FireDirection { FRONT, DOWN, UP, NEAR_BY }


public class PlayerGun : MonoBehaviour
{
    public PlayerBullet bulletPrefab;
    public Transform firePoint;
    public int poolSize = 20;

    Transform bulletPoolParent;

    public int maxMagazine = 20;
    public int nowMagazine = 20;

    private Queue<PlayerBullet> bulletPool = new Queue<PlayerBullet>();
    private Dictionary<FireDirection, Vector2> directionDict = new Dictionary<FireDirection, Vector2>
    {
        { FireDirection.FRONT, Vector2.right },
        { FireDirection.DOWN, new Vector2(1,-1).normalized },
        { FireDirection.UP, new Vector2(1,1).normalized },
        { FireDirection.NEAR_BY, new Vector2(0, -1).normalized }
    };

    private void Start()
    {
        // 총알 풀 초기화
        InitializeBulletPool();
    }

    private void InitializeBulletPool()
    {
        bulletPoolParent = new GameObject("BulletPool").transform;

        // 총알 풀에 총알을 생성하고 비활성화하여 추가
        for (int i = 0; i < poolSize; i++)
        {
            PlayerBullet bullet = Instantiate(bulletPrefab, Vector3.zero, Quaternion.identity, bulletPoolParent);
            bullet.Init(this);
        }
    }

    public void Shoot(FireDirection fireDirection, Direction playerDirection)
    {
        // 총알 발사
        if (bulletPool.Count > 0 && nowMagazine > 0)
        {
            PlayerBullet bullet = bulletPool.Dequeue();
            bullet.gameObject.SetActive(true);

            Vector2 direction = directionDict[fireDirection];

            if (playerDirection == Direction.LEFT)
                direction.x = -direction.x;

            bullet.Fire(firePoint.position, direction);

            nowMagazine--;
            if (nowMagazine <= 0)
            {
                StartCoroutine(Reload());
            }
        }
    }

    IEnumerator Reload()
    {
        yield return new WaitForSeconds(1);
        nowMagazine = maxMagazine;
    }

    public void EnqueueBullet(PlayerBullet bullet)
    {
        bulletPool.Enqueue(bullet);
    }
}
using UnityEngine;
using System.Collections.Generic;
using System.IO.Pipes;

public enum FireDirection { FRONT, DOWN, UP, NEAR_BY }


public class PlayerGun : MonoBehaviour
{
    public PlayerBullet bulletPrefab;
    public Transform firePoint;
    public int poolSize = 20;

    Transform bulletPoolParent;

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
        // �Ѿ� Ǯ �ʱ�ȭ
        InitializeBulletPool();
    }

    private void InitializeBulletPool()
    {
        bulletPoolParent = new GameObject("BulletPool").transform;

        // �Ѿ� Ǯ�� �Ѿ��� �����ϰ� ��Ȱ��ȭ�Ͽ� �߰�
        for (int i = 0; i < poolSize; i++)
        {
            PlayerBullet bullet = Instantiate(bulletPrefab, Vector3.zero, Quaternion.identity, bulletPoolParent);
            bullet.Init(this);
        }
    }

    public void Shoot(FireDirection fireDirection, Direction playerDirection)
    {
        // �Ѿ� �߻�
        if (bulletPool.Count > 0)
        {
            PlayerBullet bullet = bulletPool.Dequeue();
            bullet.gameObject.SetActive(true);

            Vector2 direction = directionDict[fireDirection];

            if (playerDirection == Direction.LEFT)
                direction.x = -direction.x;

            bullet.Fire(firePoint.position, direction);
        }
    }

    public void EnqueueBullet(PlayerBullet bullet)
    {
        bulletPool.Enqueue(bullet);
    }
}

using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public class PlayerGun : MonoBehaviour
{
    public List<Transform> fireAngles;

    private Dictionary<PlayerState, Transform> fireAngleDictionary = new();

    public LayerMask targetLayer;
    public float tolerance = 10;

    DamageableObject targetEnemy = null;
    Vector3 targetPoint;

    public ScopeEffect scopePrefab;
    public ScopeEffect nowScope;

    public PlayerBullet bulletPrefab;
    public Transform firePoint;
    public int poolSize = 20;

    Transform bulletPoolParent;

    public int maxMagazine = 20;
    public int nowMagazine = 20;

    private Queue<PlayerBullet> bulletPool = new Queue<PlayerBullet>();


    private void Start()
    {
        // 총알 풀 초기화
        InitializeBulletPool();

        InitializeFireAngleDictionary();
    }


    public void Update()
    {
        if (GameManager.Instance.gameMode != GameMode.NARRATIVE)
        {
            PlayerState playerState = PlayerManager.Instance.playerState;
            Vector3 playerPos = PlayerManager.Instance.player.transform.position;

            float minDistance = float.MaxValue;

            DamageableObject newTargetEnemy = null;

            foreach (var enemy in OppositionEntityManager.Instance.GetEnemyList)
            {
                if (enemy.nowHP <= 0)
                    continue;
                if (enemy.transform.position.x - playerPos.x >= 15)
                    continue;

                if (playerState != PlayerState.JUMP2)
                {
                    Vector3 targetAngle = fireAngleDictionary[playerState].position - playerPos;
                    Vector3 enemyAngle = enemy.transform.position - playerPos;
                    float angle = Vector3.SignedAngle(targetAngle, enemyAngle, Vector3.forward);

                    if (MathF.Abs(angle) > tolerance)
                        continue;
                }


                float enemyDistance = Vector3.Distance(playerPos, enemy.transform.position);

                if (enemyDistance < minDistance)
                {
                    minDistance = enemyDistance;
                    newTargetEnemy = enemy;
                }
            }

            if (newTargetEnemy != targetEnemy)
            {
                targetEnemy = newTargetEnemy;

                if (nowScope != null)
                    nowScope.Hide();

                if (targetEnemy != null)
                {
                    ScopeEffect scope = Instantiate(scopePrefab);
                    scope.Init(targetEnemy);
                    nowScope = scope;
                }
            }

            if (targetEnemy != null)
            {
                targetPoint = targetEnemy.transform.position;
            }
            else
            {
                targetPoint = fireAngleDictionary[playerState].position;
            }
        }
        else
        {
            targetEnemy = null;
        }
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

    private void InitializeFireAngleDictionary()
    {
        fireAngleDictionary = new Dictionary<PlayerState, Transform>
        {
            { PlayerState.RUN,   fireAngles[0] },
            { PlayerState.IDLE,  fireAngles[0] },
            { PlayerState.SLIDE, fireAngles[1] },
            { PlayerState.JUMP,  fireAngles[2] },
            { PlayerState.JUMP2,  fireAngles[2] },
        };
    }

    public void Shoot()
    {
        // 총알 발사
        if (bulletPool.Count > 0 && nowMagazine > 0)
        {
            PlayerBullet bullet = bulletPool.Dequeue();
            bullet.gameObject.SetActive(true);


            bullet.Fire(firePoint.position, targetPoint, targetEnemy);

            //nowMagazine--;
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Normal : Enemy
{
    public Transform firePoint;

    public override void Attack()
    {
        GameObject projectile = ProjectilePool.Instance.GetProjectile();
        projectile.GetComponent<EnemyProjectile>().Initialize(firePoint.position, firePoint.forward);
    }

    public override void DetectPlayer()
    {
        Attack();
    }
}

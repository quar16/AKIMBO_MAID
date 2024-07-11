using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : MonoSingleton<ProjectilePool>
{
    [SerializeField]
    private GameObject projectilePrefab;
    [SerializeField]
    private int poolSize = 10;

    private Queue<GameObject> poolQueue = new Queue<GameObject>();

    void Start()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject projectile = Instantiate(projectilePrefab);
            projectile.SetActive(false);
            poolQueue.Enqueue(projectile);
        }
    }

    public GameObject GetProjectile()
    {
        if (poolQueue.Count > 0)
        {
            GameObject projectile = poolQueue.Dequeue();
            projectile.SetActive(true);
            return projectile;
        }
        else
        {
            GameObject projectile = Instantiate(projectilePrefab);
            projectile.SetActive(false);
            return projectile;
        }
    }

    public void ReturnProjectile(GameObject projectile)
    {
        projectile.SetActive(false);
        poolQueue.Enqueue(projectile);
    }
}

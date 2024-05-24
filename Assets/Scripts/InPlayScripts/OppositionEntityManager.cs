using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OppositionEntityManager : MonoSingleton<OppositionEntityManager>
{
    public float maxCoolTime;
    public List<DamageableObject> prefabs;
    List<DamageableObject> spawnedEnemies = new List<DamageableObject>();
    float coolTime = 0;

    void Update()
    {
        coolTime -= Time.deltaTime;

        if (coolTime < 0)
        {
            coolTime = maxCoolTime;

            DamageableObject enemy = Instantiate(prefabs[Random.Range(0, prefabs.Count)]);

            enemy.transform.position = PlayerManager.Instance.player.transform.position
                + new Vector3(Random.Range(20, 40f), Random.Range(0, 5f), 0);

            spawnedEnemies.Add(enemy);
        }
    }

    public List<DamageableObject> GetEnemyList
    {
        get
        {
            spawnedEnemies.RemoveAll(obj => obj == null);

            return spawnedEnemies;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OppositionEntityManager : MonoSingleton<OppositionEntityManager>
{
    public float maxCoolTime;
    public List<DamageableObject> prefabs;

    int enemySpawnGap = 25;

    List<DamageableObject> spawnedEnemies = new List<DamageableObject>();

    public List<DamageableObject> GetEnemyList
    {
        get
        {
            spawnedEnemies.RemoveAll(obj => obj == null);

            return spawnedEnemies;
        }
    }

    public void StartEnemySpawnRoutine(List<EntitySpawnData> entitySpawnDataList)
    {
        StartCoroutine(EnemySpawnRoutine(entitySpawnDataList));
    }

    private IEnumerator EnemySpawnRoutine(List<EntitySpawnData> entitySpawnDataList)
    {
        foreach (var entity in entitySpawnDataList)
        {
            yield return new WaitUntil(() => entity.gridIndex.x < PlayerManager.Instance.player.transform.position.x);

            DamageableObject enemy = Instantiate(prefabs[entity.prefabId]);

            enemy.transform.position = new Vector3(entity.gridIndex.x + enemySpawnGap, entity.gridIndex.y, 0);

            spawnedEnemies.Add(enemy);
        }

        yield break;
    }


}

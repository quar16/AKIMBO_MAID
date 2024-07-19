using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OppositionEntityManager : MonoSingleton<OppositionEntityManager>
{
    public List<DamageableObject> prefabs;
    Dictionary<int, DamageableObject> prefabDicitionary = new();

    float enemySpawnGap = 34;

    List<DamageableObject> spawnedEntities = new List<DamageableObject>();

    public List<DamageableObject> GetEnemyList
    {
        get
        {
            spawnedEntities.RemoveAll(obj => obj == null);

            return spawnedEntities;
        }
    }

    public void Start()
    {
        foreach (var v in prefabs)
        {
            prefabDicitionary.Add(v.prefabId, v);
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
            yield return new WaitUntil(() => entity.gridIndex.x < PlayerManager.Instance.player.transform.position.x + enemySpawnGap);

            DamageableObject tempDobj = Instantiate(prefabDicitionary[entity.prefabId]);

            tempDobj.transform.position = new Vector3(entity.gridIndex.x, entity.gridIndex.y, 0);
            tempDobj.Init(entity.customValues);

            spawnedEntities.Add(tempDobj);
        }

        yield break;
    }


}

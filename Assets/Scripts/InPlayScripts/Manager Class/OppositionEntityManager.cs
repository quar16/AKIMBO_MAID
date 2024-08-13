using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OppositionEntityManager : MonoSingleton<OppositionEntityManager>
{
    public List<Entity> prefabs;
    Dictionary<int, Entity> prefabDicitionary = new();

    float enemySpawnGap = 34; // 스테이지 길이의 두배

    IEnumerator entitySpawnCoroutine;
    List<Entity> spawnedEntities = new();

    public List<Entity> GetEnemyList
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

    public void Init(List<EntitySpawnData> entitySpawnDataList)
    {
        foreach (var v in prefabs)
        {
            if (!prefabDicitionary.ContainsKey(v.prefabId))
                prefabDicitionary.Add(v.prefabId, v);
        }

        entitySpawnCoroutine = EntitySpawnRoutine(entitySpawnDataList);
        StartCoroutine(entitySpawnCoroutine);
    }

    private IEnumerator EntitySpawnRoutine(List<EntitySpawnData> entitySpawnDataList)
    {
        foreach (var entitySpawnData in entitySpawnDataList)
        {
            yield return new WaitUntil(() => entitySpawnData.gridIndex.x < PlayerManager.Instance.player.transform.position.x + enemySpawnGap);

            SpawnEntity(entitySpawnData);
        }

        yield break;
    }

    public void SpawnEntity(EntitySpawnData entitySpawnData)
    {
        Entity tempDobj = this.Instantiate(prefabDicitionary[entitySpawnData.prefabId]);

        tempDobj.transform.position = new Vector3(entitySpawnData.gridIndex.x, entitySpawnData.gridIndex.y, 0);
        tempDobj.Init(entitySpawnData.customValues);

        spawnedEntities.Add(tempDobj);
    }

    public void SpawnEntity(Entity entity, Vector3 position)
    {
        Entity tempDobj = this.Instantiate(entity);

        tempDobj.transform.position = position;

        spawnedEntities.Add(tempDobj);
    }

    public void DespawnEntity(Entity despawnEntity)
    {
        if (spawnedEntities.Contains(despawnEntity))
            spawnedEntities.Remove(despawnEntity);

        Destroy(despawnEntity.gameObject);
    }
}

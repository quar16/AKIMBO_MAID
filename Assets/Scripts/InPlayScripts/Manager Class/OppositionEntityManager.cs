using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OppositionEntityManager : MonoSingleton<OppositionEntityManager>
{
    public List<DamageableObject> prefabs;
    Dictionary<int, DamageableObject> prefabDicitionary = new();

    float enemySpawnGap = 34; // 스테이지 길이의 두배

    IEnumerator entitySpawnCoroutine;
    List<DamageableObject> spawnedEntities = new();

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

    public void Init(List<EntitySpawnData> entitySpawnDataList)
    {
        entitySpawnCoroutine = EntitySpawnRoutine(entitySpawnDataList);
        StartCoroutine(entitySpawnCoroutine);
    }

    private IEnumerator EntitySpawnRoutine(List<EntitySpawnData> entitySpawnDataList)
    {
        foreach (var entity in entitySpawnDataList)
        {
            yield return new WaitUntil(() => entity.gridIndex.x < PlayerManager.Instance.player.transform.position.x + enemySpawnGap);

            DamageableObject tempDobj = this.Instantiate(prefabDicitionary[entity.prefabId]);

            tempDobj.transform.position = new Vector3(entity.gridIndex.x, entity.gridIndex.y, 0);
            tempDobj.Init(entity.customValues);

            spawnedEntities.Add(tempDobj);
        }

        yield break;
    }


    public void CleanUp()
    {
        StopCoroutine(entitySpawnCoroutine);

        foreach (var v in spawnedEntities)
            Destroy(v.gameObject);

        spawnedEntities.Clear();
    }
}

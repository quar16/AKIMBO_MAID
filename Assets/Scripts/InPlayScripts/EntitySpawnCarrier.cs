using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitySpawnCarrier : MonoBehaviour
{
    public DamageableObject entity;

    private void Start()
    {
        OppositionEntityManager.Instance.SpawnEntity(entity, transform.position);
        Destroy(gameObject);
    }
}

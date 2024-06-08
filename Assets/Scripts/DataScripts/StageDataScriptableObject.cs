using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Stage - 1", menuName = "Stage Data")]
public class StageDataScriptableObject : ScriptableObject
{
    public GameObject floorSprite;
    public GameObject wallSprite;
    public GameObject backgroundSprite;

    public List<EntitySpawnData> entities = new();
}

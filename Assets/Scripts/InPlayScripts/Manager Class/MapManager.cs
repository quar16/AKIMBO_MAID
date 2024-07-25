using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoSingleton<MapManager>
{
    public List<SpecialMap> specialMapList = new();
    Dictionary<MapIndex, SpecialMap> specialMapDictionary = new();

    List<SpecialMap> spawnedSpecialMap = new();

    public List<BackGroundMover> backGroundList = new();
    public BackGroundMover floor;
    public BackGroundMover wall;

    private void Start()
    {
        foreach (var v in specialMapList)
            specialMapDictionary.Add(v.mapIndex, v);
    }

    public void Init(GameObject floorPrefab, GameObject wallPrefab)
    {
        floor.backgroundPrefab = floorPrefab;
        wall.backgroundPrefab = wallPrefab;

        foreach (var v in backGroundList)
        {
            v.Init();
        }
    }

    public void CleanUp()
    {
        foreach (var v in spawnedSpecialMap)
        {
            Destroy(v.gameObject);
        }
        spawnedSpecialMap.Clear();

        foreach (var v in backGroundList)
        {
            v.CleanUp();
        }
    }

    public void AddRemoveIndex(int index)
    {
        floor.removeIndex.Add(index);
        wall.removeIndex.Add(index);

        foreach (var v in floor.backGroundDic)
            if (v.Key == index)
                v.Value.SetActive(false);
        foreach (var v in wall.backGroundDic)
            if (v.Key == index)
                v.Value.SetActive(false);
    }

    public void SpawnSpecialMap(MapIndex mapIndex, int posIndex)
    {
        SpecialMap sMap = Instantiate(specialMapDictionary[mapIndex], new Vector3(posIndex * 17, 0, 0), Quaternion.identity, null);
        spawnedSpecialMap.Add(sMap);
    }

}

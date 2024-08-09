using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MapManager : MonoSingleton<MapManager>
{
    public List<SpecialMap> specialMapList = new();
    Dictionary<MapIndex, SpecialMap> specialMapDictionary = new();

    List<SpecialMap> spawnedSpecialMap = new();

    public Transform backGroundMoverParent;

    public List<BackGroundMover> backGroundList = new();
    public BackGroundMover floor;
    public BackGroundMover wall;


    private void Start()
    {
        foreach (var v in specialMapList)
            specialMapDictionary.Add(v.mapIndex, v);
    }

    public void Init(BackGroundMover floorPrefab, BackGroundMover wallPrefab)
    {
        foreach (var v in backGroundList)
            v.Init();

        floor = Instantiate(floorPrefab, backGroundMoverParent);
        wall = Instantiate(wallPrefab, backGroundMoverParent);

        floor.Init();
        wall.Init();
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
        floor.AddRemoveIndex(index);
        wall.AddRemoveIndex(index);
    }

    public void AddRemoveIndex_Stage2(int index, BlockStateD blockState)
    {
        if (blockState == BlockStateD.None)
            floor.AddRemoveIndex(index);
        else if (blockState == BlockStateD.Broken)
            if (floor.TryGetComponent(out BackGroundMover_Stage2_Floor backGroundMover))
                backGroundMover.AddBrokenIndex(index);
    }

    public void SpawnSpecialMap(MapIndex mapIndex, int posIndex)
    {
        SpecialMap sMap = this.Instantiate(specialMapDictionary[mapIndex], new Vector3(posIndex * 17, 0, 0), Quaternion.identity);
        spawnedSpecialMap.Add(sMap);
    }

}

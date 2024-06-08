using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;
using System.Collections.Generic;

public class PrefabLoader : MonoSingleton<PrefabLoader>
{
    // 프리팹이 있는 폴더 경로
    public string prefabFolderName;

    string PathForDirectoryGetFiles { get { return "Assets/StageEditor/Resources/" + prefabFolderName; } }
    string PathForResourcesLoad { get { return prefabFolderName + "/"; } }

    // UI 리스트를 나타내는 변수
    public CustomDropdown prefabDropdown;
    public Transform entityParent;
    public List<DraggableObject> entitiesList = new();

    // Start 메서드에서 프리팹 목록을 읽어와서 UI 리스트에 추가합니다.
    private void Start()
    {
        LoadPrefabList();
    }

    // 폴더 내의 프리팹 목록을 읽어와서 UI 리스트에 추가하는 함수
    private void LoadPrefabList()
    {
        // 폴더에서 모든 프리팹 파일 경로 가져오기
        string[] prefabPaths = Directory.GetFiles(PathForDirectoryGetFiles, "*.prefab");

        // 각 프리팹 파일 경로를 UI 드롭다운에 추가
        foreach (string prefabPath in prefabPaths)
        {
            // 프리팹 파일명 가져오기
            string prefabName = Path.GetFileNameWithoutExtension(prefabPath);
            // 드롭다운 옵션으로 추가
            prefabDropdown.AddItem(prefabName);
        }
    }

    // 사용자가 선택한 프리팹을 로드하여 씬에 배치하는 함수
    public void LoadSelectedPrefab()
    {
        // 선택된 드롭다운 옵션 인덱스 가져오기
        int selectedIndex = prefabDropdown.Value;

        // 선택된 프리팹 파일 경로 가져오기
        string selectedPrefabPath = PathForResourcesLoad + prefabDropdown.GetItemByIndex(selectedIndex);

        //카메라 중앙에 소환
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, Camera.main.nearClipPlane);
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenCenter);
        worldPosition.z = 0;

        SpawnEntity(Resources.Load<DraggableObject>(selectedPrefabPath), worldPosition);
    }

    public void SpawnEntity(DraggableObject prefab, Vector3 position)
    {
        DraggableObject dObj = Instantiate(prefab, entityParent);

        dObj.transform.position = position;

        //이닛 호출
        dObj.Init();

        entitiesList.Add(dObj);
    }

    public void SpawnEntity(EntitySpawnData spawnData)
    {
        string selectedPrefabPath = PathForResourcesLoad + prefabDropdown.GetItemByIndex(spawnData.prefabId);

        Vector3 worldPosition = TileGrid.GetTilePosByGridIndex(spawnData.gridIndex);

        SpawnEntity(Resources.Load<DraggableObject>(selectedPrefabPath), worldPosition);
    }

    public void UnspawnEntity(DraggableObject unspawnTarget)
    {
        if (entitiesList.Contains(unspawnTarget))
        {
            entitiesList.Remove(unspawnTarget);
            Destroy(unspawnTarget.gameObject);
        }
    }

    public void ResetEntityList()
    {
        foreach (var entity in entitiesList)
            Destroy(entity.gameObject);

        entitiesList.Clear();
    }
}

using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class PrefabLoader : MonoSingleton<PrefabLoader>
{
    // 프리팹이 있는 폴더 경로
    public string prefabFolderName;

    string PathForDirectoryGetFiles { get { return "Assets/StageEditor/Resources/" + prefabFolderName; } }
    string PathForResourcesLoad { get { return prefabFolderName + "/"; } }

    // UI 리스트를 나타내는 변수
    public CustomDropdown prefabDropdown;
    public Dictionary<int, DraggableObject> prefabDic = new();
    public Transform entityParent;
    public List<DraggableObject> entitiesList = new();

    // Start 메서드에서 프리팹 목록을 읽어와서 UI 리스트에 추가합니다.
    private void Start()
    {
        LoadPrefabList();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D collider = Physics2D.OverlapPoint(mousePosition);

            if (collider != null || EventSystem.current.IsPointerOverGameObject())
                return;

            LoadSelectedPrefab(mousePosition - Vector2.one * 0.5f);
        }
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

            string selectedPrefabPath = PathForResourcesLoad + prefabName;
            DraggableObject dobj = Resources.Load<DraggableObject>(selectedPrefabPath);
            prefabDic.Add(dobj.prefabId, dobj);
        }
    }

    // 사용자가 선택한 프리팹을 로드하여 씬에 배치하는 함수
    public void LoadSelectedPrefab(Vector2 point)
    {
        // 선택된 드롭다운 옵션 인덱스 가져오기
        int selectedIndex = prefabDropdown.Value;

        // 선택된 프리팹 파일 경로 가져오기
        string selectedPrefabPath = PathForResourcesLoad + prefabDropdown.GetItemByIndex(selectedIndex);

        SpawnEntity(Resources.Load<DraggableObject>(selectedPrefabPath), point);
    }

    public void SpawnEntity(DraggableObject prefab, Vector3 position, List<float> customValues = null)
    {
        DraggableObject dObj = Instantiate(prefab, entityParent);

        dObj.transform.position = position;
        dObj.customValues = customValues;

        //이닛 호출
        dObj.Init();

        entitiesList.Add(dObj);
    }

    public void SpawnEntity(EntitySpawnData spawnData)
    {
        Vector3 worldPosition = TileGrid.GetTilePosByGridIndex(spawnData.gridIndex);

        SpawnEntity(prefabDic[spawnData.prefabId], worldPosition, spawnData.customValues);
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

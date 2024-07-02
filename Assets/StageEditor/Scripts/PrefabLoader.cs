using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;
using System.Collections.Generic;

public class PrefabLoader : MonoSingleton<PrefabLoader>
{
    // �������� �ִ� ���� ���
    public string prefabFolderName;

    string PathForDirectoryGetFiles { get { return "Assets/StageEditor/Resources/" + prefabFolderName; } }
    string PathForResourcesLoad { get { return prefabFolderName + "/"; } }

    // UI ����Ʈ�� ��Ÿ���� ����
    public CustomDropdown prefabDropdown;
    public Dictionary<int, DraggableObject> prefabDic = new();
    public Transform entityParent;
    public List<DraggableObject> entitiesList = new();

    // Start �޼��忡�� ������ ����� �о�ͼ� UI ����Ʈ�� �߰��մϴ�.
    private void Start()
    {
        LoadPrefabList();
    }

    // ���� ���� ������ ����� �о�ͼ� UI ����Ʈ�� �߰��ϴ� �Լ�
    private void LoadPrefabList()
    {
        // �������� ��� ������ ���� ��� ��������
        string[] prefabPaths = Directory.GetFiles(PathForDirectoryGetFiles, "*.prefab");

        // �� ������ ���� ��θ� UI ��Ӵٿ �߰�
        foreach (string prefabPath in prefabPaths)
        {
            // ������ ���ϸ� ��������
            string prefabName = Path.GetFileNameWithoutExtension(prefabPath);
            // ��Ӵٿ� �ɼ����� �߰�
            prefabDropdown.AddItem(prefabName);

            string selectedPrefabPath = PathForResourcesLoad + prefabName;
            DraggableObject dobj = Resources.Load<DraggableObject>(selectedPrefabPath);
            prefabDic.Add(dobj.prefabId, dobj);
        }
    }

    // ����ڰ� ������ �������� �ε��Ͽ� ���� ��ġ�ϴ� �Լ�
    public void LoadSelectedPrefab()
    {
        // ���õ� ��Ӵٿ� �ɼ� �ε��� ��������
        int selectedIndex = prefabDropdown.Value;

        // ���õ� ������ ���� ��� ��������
        string selectedPrefabPath = PathForResourcesLoad + prefabDropdown.GetItemByIndex(selectedIndex);

        //ī�޶� �߾ӿ� ��ȯ
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, Camera.main.nearClipPlane);
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenCenter);
        worldPosition.z = 0;

        SpawnEntity(Resources.Load<DraggableObject>(selectedPrefabPath), worldPosition);
    }

    public void SpawnEntity(DraggableObject prefab, Vector3 position, List<float> customValues = null)
    {
        DraggableObject dObj = Instantiate(prefab, entityParent);

        dObj.transform.position = position;
        dObj.customValues = customValues;

        //�̴� ȣ��
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

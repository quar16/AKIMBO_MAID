using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class StageDataSaveLoader : MonoBehaviour
{
    public CustomDropdown stageDataDropdown;
    public TMP_InputField stageNameInputField;
    string NewStageName { get { return stageNameInputField.text; } }

    public string prefabFolderName;
    string PathForDirectoryGetFiles { get { return "Assets/StageEditor/Resources/" + prefabFolderName; } }
    string PathForResourcesLoad { get { return prefabFolderName + "/"; } }


    private void Start()
    {
        LoadStageDataList();
    }

    private void LoadStageDataList()
    {
        // �������� ��� �������� ������ ��������
        string[] prefabPaths = Directory.GetFiles(PathForDirectoryGetFiles, "*.asset");

        // �� �������� �����͸� ��Ӵٿ �߰�
        foreach (string prefabPath in prefabPaths)
        {
            string prefabName = Path.GetFileNameWithoutExtension(prefabPath);
            stageDataDropdown.AddItem(prefabName);
        }
    }

    public void LoadStageDataButton()
    {
        EditorAlternativePopUp.Instance.ShowPopUp("���ο� �������� �����͸� �ҷ����ðڽ��ϱ�? ���� �۾� ���� ������ ������ϴ�.", ApplyResponseOfLoadStageDataButton);

    }

    public void ApplyResponseOfLoadStageDataButton(bool isYes)
    {
        PrefabLoader.Instance.ResetEntityList();

        string path = PathForResourcesLoad + stageDataDropdown.GetNowItem();
        StageDataScriptableObject stageData = Resources.Load<StageDataScriptableObject>(path);

        if (stageData != null)
        {
            foreach (var v in stageData.entities)
                PrefabLoader.Instance.SpawnEntity(v);
        }
        else
        {
            Debug.LogError("ScriptableObject not found at path: " + path);
        }

        //������ �������� �����͸� ������� ��ƼƼ ���� ���ȯ
    }

    public void SaveStageDataButton()
    {
        string content = NewStageName;

        if (NewStageName == "")
        {
            Debug.LogWarning("�������� �̸��� ����� �մϴ�.");
            return;
        }
        else if (stageDataDropdown.IsItemExist(NewStageName))
        {
            content += ", �� �̸��� ���� �̸��� �������� �����Ͱ� �̹� �����մϴ�. �ش� �����Ϳ� ��������ϴ�. �����͸� �����Ͻðڽ��ϱ�?";
        }
        else
        {
            content += ", �� �̸����� �������� �����͸� �����Ͻðڽ��ϱ�?";
        }

        EditorAlternativePopUp.Instance.ShowPopUp(content, ApplyResponseOfSaveStageDataButton);
    }

    public void ApplyResponseOfSaveStageDataButton(bool isYes)
    {
        if (!isYes) return;


        StageDataScriptableObject newStageData = ScriptableObject.CreateInstance<StageDataScriptableObject>();

        if (stageDataDropdown.IsItemExist(NewStageName))
        {
            string _path = PathForResourcesLoad + NewStageName;
            StageDataScriptableObject stageData = Resources.Load<StageDataScriptableObject>(_path);

            newStageData.floorSprite = stageData.floorSprite;
            newStageData.wallSprite = stageData.wallSprite;
            newStageData.backgroundSprite = stageData.backgroundSprite;
        }


        foreach (var v in PrefabLoader.Instance.entitiesList)
        {
            EntitySpawnData spawnData = new();

            spawnData.gridIndex = v.gridIndex;
            spawnData.offset = v.cameraOffset;
            spawnData.prefabId = v.prefabId;
            spawnData.customValues = v.GetCustomValue();

            newStageData.entities.Add(spawnData);
        }

        newStageData.entities.Sort((x, y) => x.gridIndex.x != y.gridIndex.x ? x.gridIndex.x.CompareTo(y.gridIndex.x) : x.gridIndex.y.CompareTo(y.gridIndex.y));


        // ���丮�� �������� ������ ����
        if (!Directory.Exists(PathForDirectoryGetFiles))
        {
            Debug.LogWarning("directory not exist!");
            return;
        }

        // ���� ��� ����
        string path = Path.Combine(PathForDirectoryGetFiles, NewStageName + ".asset");

        // AssetDatabase�� ����Ͽ� ScriptableObject�� ����
#if UNITY_EDITOR
        UnityEditor.AssetDatabase.CreateAsset(newStageData, path);
        UnityEditor.AssetDatabase.SaveAssets();
        UnityEditor.AssetDatabase.Refresh();
#endif

        // ��Ӵٿ ���ο� �������� ������ �߰�
        stageDataDropdown.AddItem(NewStageName);
    }


    public void PlayTestButton()
    {
        if (NewStageName == "")
        {
            Debug.LogWarning("�������� �̸��� ����� �մϴ�.");
            return;
        }

        string content =NewStageName + " �� �̸����� ������ �۾� ������ �����ϰ�, �׽�Ʈ �÷��̸� �����մϴ�.";
        EditorAlternativePopUp.Instance.ShowPopUp(content, ApplyResponseOfPlayTestButton);
    }

    public void ApplyResponseOfPlayTestButton(bool isYes)
    {
        if (!isYes) return;

        ApplyResponseOfSaveStageDataButton(isYes);

        
    }
}

using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class EntityDataSaveLoader : MonoBehaviour
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
        // 폴더에서 모든 스테이지 데이터 가져오기
        string[] prefabPaths = Directory.GetFiles(PathForDirectoryGetFiles, "*.asset");

        // 각 스테이지 데이터를 드롭다운에 추가
        foreach (string prefabPath in prefabPaths)
        {
            string prefabName = Path.GetFileNameWithoutExtension(prefabPath);
            stageDataDropdown.AddItem(prefabName);
        }
    }

    public void LoadStageDataButton()
    {
        EditorAlternativePopUp.Instance.ShowPopUp("새로운 스테이지 데이터를 불러오시겠습니까? 지금 작업 중인 내용은 사라집니다.", ApplyResponseOfLoadEntityDataButton);

    }

    public void ApplyResponseOfLoadEntityDataButton(bool isYes)
    {
        if (!isYes) return;

        PrefabLoader.Instance.ResetEntityList();

        string path = PathForResourcesLoad + stageDataDropdown.GetNowItem();
        EntityDataScriptableObject entityData = Resources.Load<EntityDataScriptableObject>(path);

        if (entityData != null)
        {
            foreach (var v in entityData.entities)
                PrefabLoader.Instance.SpawnEntity(v);

            stageNameInputField.text = entityData.name;
        }
        else
        {
            Debug.LogError("ScriptableObject not found at path: " + path);
        }

        //가져온 스테이지 데이터를 기반으로 엔티티 전부 재소환
    }

    public void SaveStageDataButton()
    {
        string content = NewStageName;

        if (NewStageName == "")
        {
            Debug.LogWarning("스테이지 이름을 적어야 합니다.");
            return;
        }
        else if (stageDataDropdown.IsItemExist(NewStageName))
        {
            content += ", 이 이름과 같은 이름의 스테이지 데이터가 이미 존재합니다. 해당 데이터에 덮어씌워집니다. 데이터를 저장하시겠습니까?";
        }
        else
        {
            content += ", 이 이름으로 스테이지 데이터를 저장하시겠습니까?";
        }

        EditorAlternativePopUp.Instance.ShowPopUp(content, ApplyResponseOfSaveEntityDataButton);
    }

    public void ApplyResponseOfSaveEntityDataButton(bool isYes)
    {
        if (!isYes) return;


        EntityDataScriptableObject newEntityData = ScriptableObject.CreateInstance<EntityDataScriptableObject>();

        if (stageDataDropdown.IsItemExist(NewStageName))
        {
            string _path = PathForResourcesLoad + NewStageName;
            EntityDataScriptableObject stageData = Resources.Load<EntityDataScriptableObject>(_path);
        }


        foreach (var v in PrefabLoader.Instance.entitiesList)
        {
            EntitySpawnData spawnData = new();

            spawnData.gridIndex = v.gridIndex;
            spawnData.offset = v.cameraOffset;
            spawnData.prefabId = v.prefabId;
            spawnData.customValues = v.GetCustomValue();

            newEntityData.entities.Add(spawnData);
        }

        newEntityData.entities.Sort((x, y) => x.gridIndex.x != y.gridIndex.x ? x.gridIndex.x.CompareTo(y.gridIndex.x) : x.gridIndex.y.CompareTo(y.gridIndex.y));


        // 디렉토리가 존재하지 않으면 생성
        if (!Directory.Exists(PathForDirectoryGetFiles))
        {
            Debug.LogWarning("directory not exist!");
            return;
        }

        // 파일 경로 설정
        string path = Path.Combine(PathForDirectoryGetFiles, NewStageName + ".asset");

        // AssetDatabase를 사용하여 ScriptableObject를 저장
#if UNITY_EDITOR
        UnityEditor.AssetDatabase.CreateAsset(newEntityData, path);
        UnityEditor.AssetDatabase.SaveAssets();
        UnityEditor.AssetDatabase.Refresh();
#endif

        // 드롭다운에 새로운 스테이지 데이터 추가
        stageDataDropdown.AddItem(NewStageName);
    }


    public void PlayTestButton()
    {
        if (NewStageName == "")
        {
            Debug.LogWarning("스테이지 이름을 적어야 합니다.");
            return;
        }

        string content =NewStageName + " 이 이름으로 현재의 작업 내용을 저장하고, 테스트 플레이를 진행합니다.";
        EditorAlternativePopUp.Instance.ShowPopUp(content, ApplyResponseOfPlayTestButton);
    }

    public void ApplyResponseOfPlayTestButton(bool isYes)
    {
        if (!isYes) return;

        ApplyResponseOfSaveEntityDataButton(isYes);

        
    }
}

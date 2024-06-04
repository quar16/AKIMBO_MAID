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

    private void SaveStageData()
    {
        StageDataScriptableObject newStageData = ScriptableObject.CreateInstance<StageDataScriptableObject>();

        foreach (var v in GetEntities())
        {
            EntitySpawnData spawnData = new();

            spawnData.x = (int)v.transform.position.x;

            newStageData.entities.Add(spawnData);
        }

        // 디렉토리가 존재하지 않으면 생성
        if (!Directory.Exists(PathForDirectoryGetFiles))
        {
            Debug.LogWarning("directory not exist!");
            //Directory.CreateDirectory(PathForDirectoryGetFiles);
        }

        // 파일 경로 설정
        string path = Path.Combine(PathForDirectoryGetFiles, NewStageName + ".asset");

        // AssetDatabase를 사용하여 ScriptableObject를 저장
#if UNITY_EDITOR
        UnityEditor.AssetDatabase.CreateAsset(newStageData, path);
        UnityEditor.AssetDatabase.SaveAssets();
        UnityEditor.AssetDatabase.Refresh();
#endif

        // 드롭다운에 새로운 스테이지 데이터 추가
        stageDataDropdown.AddItem(NewStageName);
    }



    public void LoadStageDataButton()
    {
        EditorAlternativePopUp.Instance.ShowPopUp("새로운 스테이지 데이터를 불러오시겠습니까? 지금 작업 중인 내용은 사라집니다.", ApplyResponseOfLoadStageDataButton);

    }

    public void ApplyResponseOfLoadStageDataButton(bool isYes)
    {

    }

    public void SaveStageDataButton()
    {
        if (NewStageName == "")
        {
            Debug.LogWarning("스테이지 이름을 적어야 합니다.");
        }

        else if (stageDataDropdown.IsItemExist(NewStageName))
        {
            EditorAlternativePopUp.Instance.ShowPopUp(NewStageName + ", 이 이름과 같은 이름의 스테이지 데이터가 이미 존재합니다. 해당 데이터에 덮어씌워집니다. 데이터를 저장하시겠습니까?", ApplyResponseOfSaveStageDataButton);
        }
        else
        {
            EditorAlternativePopUp.Instance.ShowPopUp(NewStageName + ", 이 이름으로 스테이지 데이터를 저장하시겠습니까?", ApplyResponseOfSaveStageDataButton);
        }
    }

    public void ApplyResponseOfSaveStageDataButton(bool isYes)
    {
        if (!isYes) return;


        StageDataScriptableObject newStageData = ScriptableObject.CreateInstance<StageDataScriptableObject>();

        foreach (var v in GetEntities())
        {
            EntitySpawnData spawnData = new();

            spawnData.x = (int)v.transform.position.x;

            newStageData.entities.Add(spawnData);
        }

        // 디렉토리가 존재하지 않으면 생성
        if (!Directory.Exists(PathForDirectoryGetFiles))
        {
            Debug.LogWarning("directory not exist!");
            return;
        }
        Debug.Log("enter here/?");

        // 파일 경로 설정
        string path = Path.Combine(PathForDirectoryGetFiles, NewStageName + ".asset");

        Debug.Log("enter here/?");

        // AssetDatabase를 사용하여 ScriptableObject를 저장
#if UNITY_EDITOR
        Debug.Log("enter here/?");
        UnityEditor.AssetDatabase.CreateAsset(newStageData, path);
        Debug.Log("enter here/?");
        UnityEditor.AssetDatabase.SaveAssets();
        Debug.Log("enter here/?");
        UnityEditor.AssetDatabase.Refresh();
        Debug.Log("enter here/?");
#endif
        Debug.Log("enter here/?");

        // 드롭다운에 새로운 스테이지 데이터 추가
        stageDataDropdown.AddItem(NewStageName);
    }

    private List<DraggableObject> GetEntities()
    {
        var list = new List<DraggableObject>();

        return list;
    }


}

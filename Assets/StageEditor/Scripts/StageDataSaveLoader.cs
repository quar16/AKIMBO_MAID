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
        // �������� ��� �������� ������ ��������
        string[] prefabPaths = Directory.GetFiles(PathForDirectoryGetFiles, "*.asset");

        // �� �������� �����͸� ��Ӵٿ �߰�
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

        // ���丮�� �������� ������ ����
        if (!Directory.Exists(PathForDirectoryGetFiles))
        {
            Debug.LogWarning("directory not exist!");
            //Directory.CreateDirectory(PathForDirectoryGetFiles);
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



    public void LoadStageDataButton()
    {
        EditorAlternativePopUp.Instance.ShowPopUp("���ο� �������� �����͸� �ҷ����ðڽ��ϱ�? ���� �۾� ���� ������ ������ϴ�.", ApplyResponseOfLoadStageDataButton);

    }

    public void ApplyResponseOfLoadStageDataButton(bool isYes)
    {

    }

    public void SaveStageDataButton()
    {
        if (NewStageName == "")
        {
            Debug.LogWarning("�������� �̸��� ����� �մϴ�.");
        }

        else if (stageDataDropdown.IsItemExist(NewStageName))
        {
            EditorAlternativePopUp.Instance.ShowPopUp(NewStageName + ", �� �̸��� ���� �̸��� �������� �����Ͱ� �̹� �����մϴ�. �ش� �����Ϳ� ��������ϴ�. �����͸� �����Ͻðڽ��ϱ�?", ApplyResponseOfSaveStageDataButton);
        }
        else
        {
            EditorAlternativePopUp.Instance.ShowPopUp(NewStageName + ", �� �̸����� �������� �����͸� �����Ͻðڽ��ϱ�?", ApplyResponseOfSaveStageDataButton);
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

        // ���丮�� �������� ������ ����
        if (!Directory.Exists(PathForDirectoryGetFiles))
        {
            Debug.LogWarning("directory not exist!");
            return;
        }
        Debug.Log("enter here/?");

        // ���� ��� ����
        string path = Path.Combine(PathForDirectoryGetFiles, NewStageName + ".asset");

        Debug.Log("enter here/?");

        // AssetDatabase�� ����Ͽ� ScriptableObject�� ����
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

        // ��Ӵٿ ���ο� �������� ������ �߰�
        stageDataDropdown.AddItem(NewStageName);
    }

    private List<DraggableObject> GetEntities()
    {
        var list = new List<DraggableObject>();

        return list;
    }


}

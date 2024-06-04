using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class PrefabLoader : MonoBehaviour
{
    // �������� �ִ� ���� ���
    public string prefabFolderName;

    string PathForDirectoryGetFiles { get { return "Assets/StageEditor/Resources/" + prefabFolderName; } }
    string PathForResourcesLoad { get { return prefabFolderName + "/"; } }

    // UI ����Ʈ�� ��Ÿ���� ����
    public CustomDropdown prefabDropdown;

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
        }
    }

    // ����ڰ� ������ �������� �ε��Ͽ� ���� ��ġ�ϴ� �Լ�
    public void LoadSelectedPrefab()
    {
        // ���õ� ��Ӵٿ� �ɼ� �ε��� ��������
        int selectedIndex = prefabDropdown.Value;

        // ���õ� ������ ���� ��� ��������
        string selectedPrefabPath = PathForResourcesLoad + prefabDropdown.GetItemByIndex(selectedIndex);

        // ���õ� ������ �ε�
        GameObject prefab = Instantiate(Resources.Load<GameObject>(selectedPrefabPath));
    }
}

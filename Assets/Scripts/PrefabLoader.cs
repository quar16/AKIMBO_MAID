using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class PrefabLoader : MonoBehaviour
{
    // �������� �ִ� ���� ���
    public string prefabFolderPath = "Assets/Resources/Prefabs";

    // UI ����Ʈ�� ��Ÿ���� ����
    public TMP_Dropdown prefabDropdown;

    // Start �޼��忡�� ������ ����� �о�ͼ� UI ����Ʈ�� �߰��մϴ�.
    private void Start()
    {
        LoadPrefabList();
    }

    // ���� ���� ������ ����� �о�ͼ� UI ����Ʈ�� �߰��ϴ� �Լ�
    private void LoadPrefabList()
    {
        // �������� ��� ������ ���� ��� ��������
        string[] prefabPaths = Directory.GetFiles(prefabFolderPath, "*.prefab");

        // �� ������ ���� ��θ� UI ��Ӵٿ �߰�
        foreach (string prefabPath in prefabPaths)
        {
            // ������ ���ϸ� ��������
            string prefabName = Path.GetFileNameWithoutExtension(prefabPath);
            // ��Ӵٿ� �ɼ����� �߰�
            prefabDropdown.options.Add(new TMP_Dropdown.OptionData(prefabName));
        }

        // ����Ʈ ����
        prefabDropdown.RefreshShownValue();
    }

    // ����ڰ� ������ �������� �ε��Ͽ� ���� ��ġ�ϴ� �Լ�
    public void LoadSelectedPrefab()
    {
        // ���õ� ��Ӵٿ� �ɼ� �ε��� ��������
        int selectedIndex = prefabDropdown.value;

        // ���õ� ������ ���� ��� ��������
        string selectedPrefabPath = "Prefabs/" + prefabDropdown.options[selectedIndex].text;

        // ���õ� ������ �ε�
        GameObject prefab = Instantiate(Resources.Load<GameObject>(selectedPrefabPath));
    }
}

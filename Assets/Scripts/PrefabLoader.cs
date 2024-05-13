using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class PrefabLoader : MonoBehaviour
{
    // 프리팹이 있는 폴더 경로
    public string prefabFolderPath = "Assets/Resources/Prefabs";

    // UI 리스트를 나타내는 변수
    public TMP_Dropdown prefabDropdown;

    // Start 메서드에서 프리팹 목록을 읽어와서 UI 리스트에 추가합니다.
    private void Start()
    {
        LoadPrefabList();
    }

    // 폴더 내의 프리팹 목록을 읽어와서 UI 리스트에 추가하는 함수
    private void LoadPrefabList()
    {
        // 폴더에서 모든 프리팹 파일 경로 가져오기
        string[] prefabPaths = Directory.GetFiles(prefabFolderPath, "*.prefab");

        // 각 프리팹 파일 경로를 UI 드롭다운에 추가
        foreach (string prefabPath in prefabPaths)
        {
            // 프리팹 파일명 가져오기
            string prefabName = Path.GetFileNameWithoutExtension(prefabPath);
            // 드롭다운 옵션으로 추가
            prefabDropdown.options.Add(new TMP_Dropdown.OptionData(prefabName));
        }

        // 리스트 갱신
        prefabDropdown.RefreshShownValue();
    }

    // 사용자가 선택한 프리팹을 로드하여 씬에 배치하는 함수
    public void LoadSelectedPrefab()
    {
        // 선택된 드롭다운 옵션 인덱스 가져오기
        int selectedIndex = prefabDropdown.value;

        // 선택된 프리팹 파일 경로 가져오기
        string selectedPrefabPath = "Prefabs/" + prefabDropdown.options[selectedIndex].text;

        // 선택된 프리팹 로드
        GameObject prefab = Instantiate(Resources.Load<GameObject>(selectedPrefabPath));
    }
}

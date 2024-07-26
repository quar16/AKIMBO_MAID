using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoader : MonoBehaviour
{
    public GameObject logoPrefab; // 로고 프리팹을 저장할 변수

    private void Start()
    {
        // 로고 표시
        //ShowLogo();

        // 메인 화면 로드
        LoadMainMenu();
    }

    private void ShowLogo()
    {
        // 로고를 인스턴스화하여 화면에 표시하는 작업
        Instantiate(logoPrefab, Vector3.zero, Quaternion.identity);
    }

    private void LoadMainMenu()
    {
        // 메인 화면 씬으로 전환
        SceneTransitionManager.Instance.TransitionToScene(SCENE.None, SCENE.Main, FadeTypes.Default, FadeTypes.Default);

    }
}

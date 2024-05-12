using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoader : MonoBehaviour
{
    public GameObject logoPrefab; // �ΰ� �������� ������ ����

    private void Start()
    {
        // �ΰ� ǥ��
        //ShowLogo();

        // ���� ȭ�� �ε�
        LoadMainMenu();
    }

    private void ShowLogo()
    {
        // �ΰ� �ν��Ͻ�ȭ�Ͽ� ȭ�鿡 ǥ���ϴ� �۾�
        Instantiate(logoPrefab, Vector3.zero, Quaternion.identity);
    }

    private void LoadMainMenu()
    {
        // ���� ȭ�� ������ ��ȯ
        SceneTransitionManager.Instance.TransitionToScene(SCENE.Main);
    }
}

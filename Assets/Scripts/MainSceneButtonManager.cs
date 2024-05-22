using UnityEngine;
using UnityEngine.SceneManagement;

public class MainSceneButtonManager : MonoBehaviour
{
    public void LoadPlayScene()
    {
        SceneTransitionManager.Instance.TransitionToScene(SCENE.Main, SCENE.Play);
    }
}

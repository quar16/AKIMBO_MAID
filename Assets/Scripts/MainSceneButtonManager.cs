using UnityEngine;
using UnityEngine.SceneManagement;

public class MainSceneButtonManager : MonoBehaviour
{
    public void LoadPlayScene()
    {
        SceneTransitionManager.Instance.TransitionToScene(SCENE.Main, SCENE.Play, FadeInOutTypes.Fade_Out_Default, FadeInOutTypes.Fade_In_Default);
    }
}

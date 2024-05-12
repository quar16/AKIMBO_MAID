using UnityEngine;
using UnityEngine.SceneManagement;

public class MainSceneButtonManager : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        SceneTransitionManager.Instance.TransitionToScene(sceneName);
    }
}

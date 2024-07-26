using UnityEngine;

public class MainSceneButtonManager : MonoBehaviour
{
    public void LoadPlayScene(int stageIndex)
    {
        StageManager.stageIndex = stageIndex;

        SceneTransitionManager.Instance.TransitionToScene(SCENE.Main, SCENE.Play, FadeTypes.Default, FadeTypes.None);
    }
}

using UnityEngine;

public class MainSceneButtonManager : MonoBehaviour
{
    public void LoadPlayScene(int stageIndex)
    {
        StageManager.stageIndex = stageIndex;

        SceneTransitionManager.Instance.TransitionToScene(SCENE.Main, SCENE.Play, FadeInOutTypes.Fade_Out_Default, FadeInOutTypes.None);
    }
}

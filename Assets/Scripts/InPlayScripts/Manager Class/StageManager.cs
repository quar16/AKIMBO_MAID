using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoSingleton<StageManager>
{
    public static int stageIndex = 0;

    public List<StageDataScriptableObject> stageDataList;
    public RectTransform cutSceneParent;

    private void Start()
    {
        StageInit();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            StageCleanUp();
            stageIndex = 1;
            StageInit();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SceneTransitionManager.Instance.TransitionToNextStage();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Time.timeScale = Mathf.Clamp01(Time.timeScale - 0.1f);
            Debug.Log("Now Time Scale is " + Time.timeScale);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Time.timeScale = Mathf.Clamp01(Time.timeScale + 0.1f);
            Debug.Log("Now Time Scale is " + Time.timeScale);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            PlayQuit();
        }
    }

    public void StageInit()
    {
        StartCoroutine(Initiate());
    }

    public IEnumerator Initiate()
    {
        PlayerManager.Instance.Init();
        OppositionEntityManager.Instance.Init(stageDataList[stageIndex].entityDataScriptableObject.entities);

        Instantiate(stageDataList[stageIndex].cutSceneGroup, cutSceneParent);
        CutSceneGroup.Instance.Init();

        MapManager.Instance.Init(stageDataList[stageIndex].floorSprite, stageDataList[stageIndex].wallSprite);

        NarrativeManager.Instance.NarrativeCall(NarrativeDataPath(0));

        yield return new WaitWhile(() => NarrativeManager.Instance.IsNarrative);

        GameManager.Instance.gameMode = GameMode.RUN;
    }

    public void StageCleanUp()
    {
        PlayerManager.Instance.CleanUp();
        OppositionEntityManager.Instance.CleanUp();
        CutSceneGroup.Instance.CleanUp();
        MapManager.Instance.CleanUp();
        NarrativeManager.Instance.CleanUp();
        CameraController.Instance.CleanUp();
    }

    public string NarrativeDataPath(int index)
    {
        return stageDataList[stageIndex].narrativeDataPaths[index];
    }

    public void PlayPause()
    {
        Time.timeScale = 0f; // 게임 시간을 멈춤
    }
    public void PlayUnpause()
    {
        Time.timeScale = 1f; // 게임 시간을 멈춤
    }

    public void PlayQuit()
    {
        StartCoroutine(PlayQuitCo());
    }

    public IEnumerator PlayQuitCo()
    {
        yield return SceneTransitionManager.Instance.CallFadeEffect(FadeTypes.Default, IO.Out);

        StageCleanUp();
        SceneTransitionManager.Instance.TransitionToScene(SCENE.Play, SCENE.Main, FadeTypes.None, FadeTypes.Default);
    }

}
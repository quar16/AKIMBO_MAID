using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoSingleton<StageManager>
{
    public static int stageIndex = 0;

    public List<StageDataScriptableObject> stageDataList;
    public RectTransform cutSceneParent;

    //temp Script
    private void Start()
    {
        StageInit();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            StageCleanUp();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            stageIndex = 1;
            StageInit();
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
        //플레이어 매니저 이닛
        PlayerManager.Instance.Init();

        //엔티티 매니저 이닛
        OppositionEntityManager.Instance.Init(stageDataList[stageIndex].entityDataScriptableObject.entities);

        //컷씬 그룹 이닛
        Instantiate(stageDataList[stageIndex].cutSceneGroup, cutSceneParent);
        CutSceneGroup.Instance.Init();

        //맵 매니저 이닛
        MapManager.Instance.Init(stageDataList[stageIndex].floorSprite, stageDataList[stageIndex].wallSprite);

        //내러티브 매니저 이닛 & 스테이지 시작
        NarrativeManager.Instance.NarrativeCall(NarrativeDataPath(0));

        yield return new WaitWhile(() => NarrativeManager.Instance.IsNarrative);

        GameManager.Instance.gameMode = GameMode.RUN;
    }

    public void StageCleanUp()
    {
        //플레이어 매니저 클린
        PlayerManager.Instance.CleanUp();

        //엔티티 매니저 클린
        OppositionEntityManager.Instance.CleanUp();

        //컷씬 그룹 클린
        CutSceneGroup.Instance.CleanUp();

        //맵 매니저 클린
        MapManager.Instance.CleanUp();

        //내러티브 클린
        NarrativeManager.Instance.CleanUp();

        //카메라 클린
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
        yield return SceneTransitionManager.Instance.CallFadeEffect(FadeInOutTypes.Fade_Out_Default);

        StageCleanUp();
        SceneTransitionManager.Instance.TransitionToScene(SCENE.Play, SCENE.Main, FadeInOutTypes.None, FadeInOutTypes.Fade_In_Default);
    }

}

public class PlayTime
{
    public static float Scale { get { return Time.deltaTime * 60f; } }

    public static readonly TimeScaledNull ScaledNull = new();

    private static Dictionary<float, WaitForSeconds> _WaitForSeconds = new();

    public static WaitForSeconds ScaledFrame = new(1 / 60f);

    public static WaitForSeconds ScaledWaitForSeconds(float seconds)
    {
        if (!_WaitForSeconds.TryGetValue(seconds, out var waitForSeconds))
        {
            waitForSeconds = new WaitForSeconds(seconds);
            _WaitForSeconds.Add(seconds, waitForSeconds);
        }

        return waitForSeconds;
    }

}

public class TimeScaledNull : CustomYieldInstruction
{
    public override bool keepWaiting
    {
        get
        {
            return Time.timeScale == 0;
        }
    }
}
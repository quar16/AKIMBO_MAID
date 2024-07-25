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
}
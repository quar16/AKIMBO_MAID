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
            var v = NamedCharacter.GetNamedCharacter("Boss1");
            if (v != null)
            {
                v.GetComponent<Entity>().ChangeHP(-1000);
            }
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

        MapManager.Instance.Init(stageDataList[stageIndex].floor, stageDataList[stageIndex].wall);

        NarrativeManager.Instance.NarrativeCall(GetNarrativeData(0));

        yield return new WaitWhile(() => NarrativeManager.Instance.IsNarrative);

        GameManager.Instance.gameMode = GameMode.RUN;
    }

    public TextAsset GetNarrativeData(int index)
    {
        return stageDataList[stageIndex].narrativeDataList[index];
    }
}
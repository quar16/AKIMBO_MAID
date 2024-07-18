using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoSingleton<StageManager>
{
    public List<StageDataScriptableObject> stageDataList;
    private int stageIndex;

    //temp Script
    private void Start()
    {
        StageInit(0);
    }

    public void StageInit(int _stageIndex)
    {
        stageIndex = _stageIndex;
        StartCoroutine(Initiate());
        OppositionEntityManager.Instance.StartEnemySpawnRoutine(stageDataList[stageIndex].entityDataScriptableObject.entities);
    }

    public IEnumerator Initiate()
    {
        string firstNarrativeDataPath = stageDataList[stageIndex].narrativeDataPaths[0];
        NarrativeManager.Instance.NarrativeCall(firstNarrativeDataPath);

        yield return new WaitWhile(() => NarrativeManager.Instance.IsNarrative);

        GameManager.Instance.gameMode = GameMode.RUN;
    }
}
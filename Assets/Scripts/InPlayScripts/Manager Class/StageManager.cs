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
        NarrativeManager.Instance.NarrativeCall(NarrativeDataPath(0));

        yield return new WaitWhile(() => NarrativeManager.Instance.IsNarrative);

        GameManager.Instance.gameMode = GameMode.RUN;
    }

    public string NarrativeDataPath(int index)
    {
        return stageDataList[stageIndex].narrativeDataPaths[index];
    }
}
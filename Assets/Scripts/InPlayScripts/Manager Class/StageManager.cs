using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoSingleton<StageManager>
{
    public List<StageDataScriptableObject> stageDataList;


    //temp Script
    private void Start()
    {
        StageInit(0);
    }

    public void StageInit(int stageIndex)
    {
        StartCoroutine(Initiate());
        OppositionEntityManager.Instance.StartEnemySpawnRoutine(stageDataList[stageIndex].entityDataScriptableObject.entities);
    }

    public IEnumerator Initiate()
    {
        GameManager.Instance.gameMode = GameMode.RUN;
        yield break;
    }
}
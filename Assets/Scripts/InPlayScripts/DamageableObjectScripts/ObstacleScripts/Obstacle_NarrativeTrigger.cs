using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle_NarrativeTrigger : Obstacle
{
    int narrativeIndex;
    GameMode afterGameMode;

    public override void Init(List<float> customData)
    {
        narrativeIndex = (int)customData[0];
        afterGameMode = (GameMode)customData[1];
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(NarrativeProcessing());
        }
    }

    IEnumerator NarrativeProcessing()
    {
        NarrativeManager.Instance.NarrativeCall(StageManager.Instance.NarrativeDataPath(narrativeIndex));

        yield return new WaitWhile(() => NarrativeManager.Instance.IsNarrative);

        GameManager.Instance.gameMode = afterGameMode;
    }
}

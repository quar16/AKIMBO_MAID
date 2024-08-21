using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarrativeFunction_Boss1_Start : NarrativeFunction
{
    public Transform minT, maxT;

    protected override IEnumerator Process(string name)
    {
        PlayerManager.Instance.playerMoveController.SetClamp(minT.position.x, maxT.position.x);
        Enemy_Boss1 boss = NamedCharacter.GetNamedCharacter("Boss1").GetComponent<Enemy_Boss1>();
        boss.SetClamp(minT.position.x, maxT.position.x);
        StartCoroutine(BossReady(boss));
        yield break;
    }

    IEnumerator BossReady(Enemy_Boss1 boss)
    {
        yield return PlayTime.ScaledWaitForSeconds(1.5f);
        boss.BossInit();
    }
}

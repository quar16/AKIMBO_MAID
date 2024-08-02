using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarrativeFunction_Boss1_Start : NarrativeFunction
{
    public Transform minT, maxT;

    protected override IEnumerator Process()
    {
        Debug.Log("asdadsasdasd");
        PlayerManager.Instance.playerMoveController.SetClamp(minT.position.x, maxT.position.x);
        Enemy_Boss1 boss = NamedCharacter.GetNamedCharacter(CharacterNames.CharacterB).GetComponent<Enemy_Boss1>();
        boss.SetClamp(minT.position.x, maxT.position.x);
        StartCoroutine(Temp(boss));
        yield break;
    }

    IEnumerator Temp(Enemy_Boss1 boss)
    {
        yield return PlayTime.ScaledWaitForSeconds(1);
        boss.BossInit();
    }
}

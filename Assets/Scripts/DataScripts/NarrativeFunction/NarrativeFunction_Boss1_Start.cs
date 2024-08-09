using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarrativeFunction_Boss1_Start : NarrativeFunction
{
    public Transform minT, maxT;

    protected override IEnumerator Process()
    {
        PlayerManager.Instance.playerMoveController.SetClamp(minT.position.x, maxT.position.x);
        Enemy_Boss1 boss = NamedCharacter.GetNamedCharacter(CharacterNames.Boss_1).GetComponent<Enemy_Boss1>();
        boss.SetClamp(minT.position.x, maxT.position.x);
        boss.BossInit();
        yield break;
    }
}

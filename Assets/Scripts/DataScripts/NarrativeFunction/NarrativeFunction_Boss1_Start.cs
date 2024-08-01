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
        NamedCharacter.GetNamedCharacter(CharacterNames.CharacterB).GetComponent<Enemy_Boss1>().SetClamp(minT.position.x, maxT.position.x);
        yield break;
    }
}

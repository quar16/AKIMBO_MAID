using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NamedCharacter : MonoBehaviour
{
    static Dictionary<CharacterNames, NamedCharacter> namedCharacterDic = new();
    public static NamedCharacter GetNamedCharacter(CharacterNames name)
    {
        if (namedCharacterDic.ContainsKey(name))
            return namedCharacterDic[name];
        else
            return null;
    }

    [SerializeField]
    private CharacterNames narrativeName;
    public CharacterNames NarrativeName { get { return narrativeName; } }

    public float cameraWeight = 0;

    private void Start()
    {
        namedCharacterDic.Add(narrativeName, this);
    }

    private void OnDestroy()
    {
        namedCharacterDic.Remove(narrativeName);
    }
}

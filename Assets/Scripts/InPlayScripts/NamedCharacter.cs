using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NamedCharacter : MonoBehaviour
{
    static Dictionary<string, NamedCharacter> namedCharacterDic = new();
    public static NamedCharacter GetNamedCharacter(string name)
    {
        if (namedCharacterDic.ContainsKey(name))
            return namedCharacterDic[name];
        else
            return null;
    }

    public Animator animator;

    [SerializeField]
    string narrativeName;
    public string NarrativeName { get { return narrativeName; } }

    public float cameraWeight = 0;

    private void Start()
    {
        namedCharacterDic.Add(narrativeName, this);
        animator = GetComponentInChildren<Animator>();
    }

    private void OnDestroy()
    {
        namedCharacterDic.Remove(narrativeName);
    }
}

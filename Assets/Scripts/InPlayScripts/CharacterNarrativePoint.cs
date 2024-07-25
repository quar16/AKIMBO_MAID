using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterNarrativePoint : MonoBehaviour
{
    static Dictionary<string, CharacterNarrativePoint> narrativePointDic = new();
    public static CharacterNarrativePoint GetNarrativePoint(string name)
    {
        if (narrativePointDic.ContainsKey(name))
            return narrativePointDic[name];
        else
            return null;
    }

    [SerializeField]
    private string pointName;
    public string PointName { get { return pointName; } }

    private void Start()
    {
        narrativePointDic.Add(pointName, this);
    }

    private void OnDestroy()
    {
        narrativePointDic.Remove(pointName);
    }
}

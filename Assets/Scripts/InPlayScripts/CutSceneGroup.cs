using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSceneGroup : MonoBehaviour
{
    Dictionary<string, RectTransform> cutSceneDictionary = new();

    void Start()
    {
        foreach (RectTransform v in GetComponent<RectTransform>())
        {
            cutSceneDictionary.Add(v.gameObject.name, v);
        }
    }

    public IEnumerator CutSceneMove()
    {
        yield return null;
    }

}

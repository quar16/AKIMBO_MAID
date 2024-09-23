using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Stage - 1", menuName = "Stage Data")]
public class StageDataScriptableObject : ScriptableObject
{
    [Header("Graphic Data")]
    public BackGroundMover floor;
    public BackGroundMover wall;

    [Space(10)]
    [Header("Cut Scene Data")]
    public CutSceneGroup cutSceneGroup;

    [Space(10)]
    [Header("Entity Data")]
    public EntityDataScriptableObject entityDataScriptableObject;

    public List<TextAsset> narrativeDataList = new();

    [HideInInspector]
    public List<string> narrativeDataPaths = new List<string>();
}
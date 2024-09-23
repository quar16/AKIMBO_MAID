using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarrativeDefinition : MonoBehaviour
{

}

public enum FunctionID
{
    Stage1_Boss_Start,
    Stage1_Boss_End,
    Stage1_Bar,
}

public enum ToggleTypes
{
    None,
    Off,
    On
}

[System.Serializable]
public class NarrativeData
{
    public List<Narrative> narratives = new List<Narrative>();
}

[System.Serializable]
public class Narrative
{
    public bool isSequential;
    public bool isAuto;
    public string narrativeType;

    public Narrative(string type)
    {
        narrativeType = type;
    }
}

[System.Serializable]
public class CameraNarrative : Narrative
{
    public bool modifyCameraSize;
    public float cameraSize;
    public bool modifyOffset;
    public SerializableVector3 offset; // Using SerializableVector3 for serialization
    public bool modifyTrackingPower;
    public float camTrackingPower;
    public bool modifyCharacter;
    public string characterName;
    public ToggleTypes targeted;
    public float weight;

    public CameraNarrative() : base(nameof(CameraNarrative)) { }
}

[System.Serializable]
public class CharacterNarrative : Narrative
{
    public string characterName;
    public string narrativePoint;
    public float speedPerSec;
    public string animationState;

    public CharacterNarrative() : base(nameof(CharacterNarrative)) { }
}

[System.Serializable]
public class DialogueNarrative : Narrative
{
    public string characterName;
    public string dialogueText;

    public DialogueNarrative() : base(nameof(DialogueNarrative)) { }
}

[System.Serializable]
public class CutSceneNarrative : Narrative
{
    public string imageName;
    public ToggleTypes toggle;
    public SerializableVector3 position;
    public float moveTime;

    public CutSceneNarrative() : base(nameof(CutSceneNarrative)) { }
}

[System.Serializable]
public class CameraShakeNarrative : Narrative
{
    public SerializableVector3 shakePower;
    public float duration;
    public int frameGap;

    public CameraShakeNarrative() : base(nameof(CameraShakeNarrative)) { }
}

[System.Serializable]
public class FadeInOutNarrative : Narrative
{
    public FadeTypes fadeType;
    public IO inOutType;

    public FadeInOutNarrative() : base(nameof(FadeInOutNarrative)) { }
}

[System.Serializable]
public class TimeDelayNarrative : Narrative
{
    public float delayTime;

    public TimeDelayNarrative() : base(nameof(TimeDelayNarrative)) { }
}

[System.Serializable]
public class FunctionNarrative : Narrative
{
    public FunctionID functionID;
    public string name;

    public FunctionNarrative() : base(nameof(FunctionNarrative)) { }
}

[System.Serializable]
public struct SerializableVector3
{
    public float x;
    public float y;
    public float z;

    public SerializableVector3(float rX, float rY, float rZ)
    {
        x = rX;
        y = rY;
        z = rZ;
    }

    public SerializableVector3(Vector3 vector3)
    {
        x = vector3.x;
        y = vector3.y;
        z = vector3.z;
    }

    public Vector3 ToVector3()
    {
        return new Vector3(x, y, z);
    }
}
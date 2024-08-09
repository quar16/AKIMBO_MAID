using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;

public class NarrativeEditorWindow : EditorWindow
{
    private NarrativeData narrativeData = new NarrativeData();
    private Vector2 scrollPos;
    private static string tempFileName;
    private int draggedIndex = -1; // 드래그 중인 요소의 인덱스
    private Rect draggedRect; // 드래그 중인 요소의 Rect

    [MenuItem("Window/Narrative Editor")]
    public static void ShowWindow()
    {
        GetWindow<NarrativeEditorWindow>("Narrative Editor");
        tempFileName = "";
    }

    private void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Camera Narrative"))
        {
            Undo.RecordObject(this, "Add New Camera Narrative");
            narrativeData.narratives.Add(new CameraNarrative());
        }

        if (GUILayout.Button("Character Narrative"))
        {
            Undo.RecordObject(this, "Add New Character Narrative");
            narrativeData.narratives.Add(new CharacterNarrative());
        }

        if (GUILayout.Button("Dialogue Narrative"))
        {
            Undo.RecordObject(this, "Add New Dialogue Narrative");
            narrativeData.narratives.Add(new DialogueNarrative());
        }

        if (GUILayout.Button("Function Narrative"))
        {
            Undo.RecordObject(this, "Add New Function Narrative");
            narrativeData.narratives.Add(new FunctionNarrative());
        }

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("CutScene Narrative"))
        {
            Undo.RecordObject(this, "Add New CutScene Narrative");
            narrativeData.narratives.Add(new CutSceneNarrative());
        }

        if (GUILayout.Button("CameraShake Narrative"))
        {
            Undo.RecordObject(this, "Add New CameraShake Narrative");
            narrativeData.narratives.Add(new CameraShakeNarrative());
        }

        if (GUILayout.Button("FadeInOut Narrative"))
        {
            Undo.RecordObject(this, "Add New FadeInOut Narrative");
            narrativeData.narratives.Add(new FadeInOutNarrative());
        }

        if (GUILayout.Button("TimeDelay Narrative"))
        {
            Undo.RecordObject(this, "Add New TimeDelay Narrative");
            narrativeData.narratives.Add(new TimeDelayNarrative());
        }

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Save Narrative"))
        {
            SaveNarrativeData();
        }

        if (GUILayout.Button("Load Narrative"))
        {
            LoadNarrativeData();
        }
        EditorGUILayout.EndHorizontal();

        // 내러티브 리스트 표시
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        for (int i = 0; i < narrativeData.narratives.Count; i++)
        {
            var narrative = narrativeData.narratives[i];
            Rect rect = EditorGUILayout.BeginVertical("box");

            DrawNarrative(narrative, i, rect);

            EditorGUILayout.EndVertical();

            HandleDragAndDrop(rect, i);
        }

        EditorGUILayout.EndScrollView();

        HandleDragging();
    }

    void DrawNarrative(Narrative narrative, int index, Rect rect)
    {
        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.LabelField((index + 1) + " - " + narrative.narrativeType);

        GUILayout.FlexibleSpace(); // Add flexible space to push the button to the right

        // 토글 버튼 추가
        narrative.isSequential = GUILayoutToggle("Sequential", narrative.isSequential);
        if (narrative.isSequential == false)
            narrative.isAuto = GUILayoutToggle("Auto", narrative.isAuto);

        if (GUILayout.Button("×", GUILayout.Width(20), GUILayout.Height(20)))
        {
            Undo.RecordObject(this, "Delete Narrative");
            narrativeData.narratives.RemoveAt(index);
        }
        EditorGUILayout.EndHorizontal();

        Undo.RecordObject(this, "Change Narrative");

        EditorGUI.indentLevel++;

        if (narrative is CameraNarrative)
            DrawCameraAction((CameraNarrative)narrative);
        else if (narrative is CharacterNarrative)
            DrawCharacterAction((CharacterNarrative)narrative);
        else if (narrative is DialogueNarrative)
            DrawDialogueAction((DialogueNarrative)narrative);
        else if (narrative is FunctionNarrative)
            DrawFunctionAction((FunctionNarrative)narrative);
        else if (narrative is CutSceneNarrative)
            DrawCutSceneAction((CutSceneNarrative)narrative);
        else if (narrative is CameraShakeNarrative)
            DrawCameraShakeAction((CameraShakeNarrative)narrative);
        else if (narrative is FadeInOutNarrative)
            DrawFadeInOutAction((FadeInOutNarrative)narrative);
        else if (narrative is TimeDelayNarrative)
            DrawTimeDelayAction((TimeDelayNarrative)narrative);

        EditorGUI.indentLevel--;
    }

    void HandleDragAndDrop(Rect rect, int index)
    {
        Event evt = Event.current;
        switch (evt.type)
        {
            case EventType.MouseDown:
                if (rect.Contains(evt.mousePosition))
                {
                    draggedIndex = index;
                    draggedRect = rect;
                    evt.Use();
                }
                break;
            case EventType.MouseUp:
                if (draggedIndex != -1 && rect.Contains(evt.mousePosition))
                {
                    SwapNarratives(draggedIndex, index);
                    draggedIndex = -1;
                }
                break;
        }
    }

    void HandleDragging()
    {
        if (draggedIndex != -1)
        {
            GUI.Box(new Rect(Event.current.mousePosition.x - draggedRect.width * 0.5f, Event.current.mousePosition.y, draggedRect.width, draggedRect.height), narrativeData.narratives[draggedIndex].narrativeType, "box");
            Repaint();
        }
    }

    void DrawCameraAction(CameraNarrative action)
    {
        EditorGUILayout.BeginHorizontal();
        action.modifyCameraSize = GUILayoutToggle("Camera Size", action.modifyCameraSize);
        GUILayout.FlexibleSpace();
        if (action.modifyCameraSize)
        {
            action.cameraSize = EditorGUILayout.FloatField("", action.cameraSize);
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        action.modifyOffset = GUILayoutToggle("Offset", action.modifyOffset);
        GUILayout.FlexibleSpace();
        if (action.modifyOffset)
        {
            EditorGUIUtility.wideMode = true;
            action.offset = new SerializableVector3(EditorGUILayout.Vector2Field("", action.offset.ToVector3()));
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        action.modifyTrackingPower = GUILayoutToggle("Tracking Power", action.modifyTrackingPower);
        GUILayout.FlexibleSpace();
        if (action.modifyTrackingPower)
        {
            action.camTrackingPower = EditorGUILayout.FloatField("", action.camTrackingPower);
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        action.modifyCharacter = GUILayoutToggle("Modify Character", action.modifyCharacter);
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
        if (action.modifyCharacter)
        {
            EditorGUI.indentLevel++;
            action.characterName = (CharacterNames)EditorGUILayout.EnumPopup("Character Name", action.characterName);
            action.targeted = (ToggleTypes)EditorGUILayout.EnumPopup("Targeted", action.targeted);
            action.weight = EditorGUILayout.FloatField("Weight", action.weight);
            EditorGUI.indentLevel--;
        }
    }

    void DrawCharacterAction(CharacterNarrative action)
    {
        action.characterName = (CharacterNames)EditorGUILayout.EnumPopup("Character Name", action.characterName);
        action.narrativePoint = EditorGUILayout.TextField("Narrative Point", action.narrativePoint);
        action.speedPerSec = EditorGUILayout.FloatField("Speed Per Second", action.speedPerSec);
        action.animationState = EditorGUILayout.TextField("Animation State", action.animationState);
    }

    void DrawDialogueAction(DialogueNarrative action)
    {
        action.characterName = (CharacterNames)EditorGUILayout.EnumPopup("Character Name", action.characterName);
        action.dialogueText = EditorGUILayout.TextField("Dialogue Text", action.dialogueText);
    }

    void DrawFunctionAction(FunctionNarrative action)
    {
        action.functionID = (FunctionID)EditorGUILayout.EnumPopup("Function ID", action.functionID);
    }

    void DrawCutSceneAction(CutSceneNarrative action)
    {
        action.imageName = EditorGUILayout.TextField("CutScene Image Name", action.imageName);
        action.toggle = (ToggleTypes)EditorGUILayout.EnumPopup("Toggle", action.toggle);
        action.position = new SerializableVector3(EditorGUILayout.Vector2Field("Position", action.position.ToVector3()));
        action.moveTime = EditorGUILayout.FloatField("Move Time", action.moveTime);
    }

    void DrawCameraShakeAction(CameraShakeNarrative action)
    {
        action.shakePower = new SerializableVector3(EditorGUILayout.Vector2Field("Shake Power", action.shakePower.ToVector3()));
        action.duration = EditorGUILayout.FloatField("Duration", action.duration);
        action.frameGap = EditorGUILayout.IntField("Frame Gap", action.frameGap);
    }

    void DrawFadeInOutAction(FadeInOutNarrative action)
    {
        action.fadeType = (FadeTypes)EditorGUILayout.EnumPopup("Fade Type", action.fadeType);
        action.inOutType = (IO)EditorGUILayout.EnumPopup("InOut Type", action.inOutType);
    }

    void DrawTimeDelayAction(TimeDelayNarrative action)
    {
        action.delayTime = EditorGUILayout.FloatField("Delay Time", action.delayTime);
    }

    void SwapNarratives(int indexA, int indexB)
    {
        var temp = narrativeData.narratives[indexA];
        narrativeData.narratives.RemoveAt(indexA);
        narrativeData.narratives.Insert(indexB, temp);
    }

    bool GUILayoutToggle(string content, bool value)
    {
        GUIStyle toggleButtonStyle = new(GUI.skin.button);
        toggleButtonStyle.normal = value ? toggleButtonStyle.onActive : toggleButtonStyle.normal;
        toggleButtonStyle.active = value ? toggleButtonStyle.onNormal : toggleButtonStyle.active;

        return GUILayout.Toggle(value, content, toggleButtonStyle);
    }

    private void SaveNarrativeData()
    {
        string path = "Assets/NarrativeData";
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        string filePath = EditorUtility.SaveFilePanel("Save Narrative Data", path, tempFileName != "" ? tempFileName : "narrativeData", "dat");

        if (filePath.Length > 0)
        {
            try
            {
                using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(fileStream, narrativeData);
                    Debug.Log("Data saved to " + filePath);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("Failed to save data: " + ex.Message);
            }
        }
        else
        {
            Debug.LogWarning("Save operation cancelled or invalid file path.");
        }
    }

    private void LoadNarrativeData()
    {
        string filePath = EditorUtility.OpenFilePanel("Load Narrative Data", "Assets/NarrativeData", "dat");
        if (filePath.Length > 0)
        {
            try
            {
                using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    narrativeData = (NarrativeData)formatter.Deserialize(fileStream);

                    tempFileName = Path.GetFileName(filePath);

                    Debug.Log("Data loaded from " + filePath);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("Failed to load data: " + ex.Message);
            }
        }
    }
}

public enum CharacterNames
{
    Player = 0,
    Bartender = 100,
    Bar_Eleveator,
    Bar_Shelf,
    Bar_Desk,
    Bar_Wall,
    Boss_1_Room_Center = 200,
    Boss_1,
    Boss_1_1,
    Boss_1_2,
    Boss_1_3,
    Boss_1_4,
}

public enum FunctionID
{
    Stage1_Boss_Start,
    Stage1_Boss_End,
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
    public CharacterNames characterName;
    public ToggleTypes targeted;
    public float weight;

    public CameraNarrative() : base(nameof(CameraNarrative)) { }
}

[System.Serializable]
public class CharacterNarrative : Narrative
{
    public CharacterNames characterName;
    public string narrativePoint;
    public float speedPerSec;
    public string animationState;

    public CharacterNarrative() : base(nameof(CharacterNarrative)) { }
}

[System.Serializable]
public class DialogueNarrative : Narrative
{
    public CharacterNames characterName;
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

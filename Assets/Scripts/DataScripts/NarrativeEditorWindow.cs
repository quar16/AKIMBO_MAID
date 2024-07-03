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

    public enum CharacterNames
    {
        CharacterA,
        CharacterB,
        CharacterC
    }

    public enum BubbleTypes
    {
        Normal,
        Shout,
        Whisper
    }

    [MenuItem("Window/Narrative Editor")]
    public static void ShowWindow()
    {
        GetWindow<NarrativeEditorWindow>("Narrative Editor");
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
            EditorGUILayout.BeginVertical("box");
            DrawNarrative(narrative, i);
            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.EndScrollView();
    }

    void DrawNarrative(Narrative narrative, int index)
    {
        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.LabelField((index + 1) + " - " + narrative.narrativeType);

        GUILayout.FlexibleSpace(); // Add flexible space to push the button to the right

        // 토글 버튼 추가
        narrative.isSequential = GUILayoutToggle("Sequential", narrative.isSequential);

        if (GUILayout.Button("↑", GUILayout.Width(20), GUILayout.Height(20)) && index > 0)
        {
            Undo.RecordObject(this, "Move Narrative Up");
            SwapNarratives(index, index - 1);
        }
        if (GUILayout.Button("↓", GUILayout.Width(20), GUILayout.Height(20)) && index < narrativeData.narratives.Count - 1)
        {
            Undo.RecordObject(this, "Move Narrative Down");
            SwapNarratives(index, index + 1);
        }
        if (GUILayout.Button("×", GUILayout.Width(20), GUILayout.Height(20)))
        {
            Undo.RecordObject(this, "Delete Narrative");
            narrativeData.narratives.RemoveAt(index);
        }
        EditorGUILayout.EndHorizontal();

        Undo.RecordObject(this, "Change Narrative");

        EditorGUI.indentLevel++;

        if (narrative is CameraNarrative)
        {
            DrawCameraAction((CameraNarrative)narrative);
        }
        else if (narrative is CharacterNarrative)
        {
            DrawCharacterAction((CharacterNarrative)narrative);
        }
        else if (narrative is DialogueNarrative)
        {
            DrawDialogueAction((DialogueNarrative)narrative);
        }

        EditorGUI.indentLevel--;
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
            action.targeted = EditorGUILayout.Toggle("Targeted", action.targeted);
            action.weight = EditorGUILayout.FloatField("Weight", action.weight);
            EditorGUI.indentLevel--;
        }
    }

    void DrawCharacterAction(CharacterNarrative action)
    {
        action.characterName = (CharacterNames)EditorGUILayout.EnumPopup("Character Name", action.characterName);
        action.position = new SerializableVector3(EditorGUILayout.Vector3Field("Position", action.position.ToVector3()));
        action.animationState = EditorGUILayout.TextField("Animation State", action.animationState);
    }

    void DrawDialogueAction(DialogueNarrative action)
    {
        action.characterName = (CharacterNames)EditorGUILayout.EnumPopup("Character Name", action.characterName);
        action.dialogueText = EditorGUILayout.TextField("Dialogue Text", action.dialogueText);
        action.bubbleType = (BubbleTypes)EditorGUILayout.EnumPopup("Bubble Type", action.bubbleType);
    }

    void SwapNarratives(int indexA, int indexB)
    {
        var temp = narrativeData.narratives[indexA];
        narrativeData.narratives[indexA] = narrativeData.narratives[indexB];
        narrativeData.narratives[indexB] = temp;
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

        string filePath = EditorUtility.SaveFilePanel("Save Narrative Data", path, "narrativeData", "dat");
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

[System.Serializable]
public class NarrativeData
{
    public List<Narrative> narratives = new List<Narrative>();
}

[System.Serializable]
public class Narrative
{
    public bool isSequential;
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
    public NarrativeEditorWindow.CharacterNames characterName;
    public bool targeted;
    public float weight;

    public CameraNarrative() : base(nameof(CameraNarrative)) { }
}

[System.Serializable]
public class CharacterNarrative : Narrative
{
    public NarrativeEditorWindow.CharacterNames characterName;
    public SerializableVector3 position;
    public string animationState;

    public CharacterNarrative() : base(nameof(CharacterNarrative)) { }
}

[System.Serializable]
public class DialogueNarrative : Narrative
{
    public NarrativeEditorWindow.CharacterNames characterName;
    public string dialogueText;
    public NarrativeEditorWindow.BubbleTypes bubbleType;

    public DialogueNarrative() : base(nameof(DialogueNarrative)) { }
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

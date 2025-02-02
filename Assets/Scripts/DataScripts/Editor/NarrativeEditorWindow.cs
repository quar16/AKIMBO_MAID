using UnityEngine;
using UnityEditor;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using Newtonsoft.Json;

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
        if (GUILayout.Button("Save Narrative AS"))
        {
            SaveNarrativeData_Asset();
        }

        if (GUILayout.Button("Load Narrative AS"))
        {
            LoadNarrativeData_Asset();
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
            action.characterName = EditorGUILayout.TextField("Character Name", action.characterName);
            action.targeted = (ToggleTypes)EditorGUILayout.EnumPopup("Targeted", action.targeted);
            action.weight = EditorGUILayout.FloatField("Weight", action.weight);
            EditorGUI.indentLevel--;
        }
    }

    void DrawCharacterAction(CharacterNarrative action)
    {
        action.characterName = EditorGUILayout.TextField("Character Name", action.characterName);
        action.narrativePoint = EditorGUILayout.TextField("Narrative Point", action.narrativePoint);
        action.speedPerSec = EditorGUILayout.FloatField("Speed Per Second", action.speedPerSec);
        action.animationState = EditorGUILayout.TextField("Animation State", action.animationState);
    }

    void DrawDialogueAction(DialogueNarrative action)
    {
        action.characterName = EditorGUILayout.TextField("Character Name", action.characterName);
        action.dialogueText = EditorGUILayout.TextField("Dialogue Text", action.dialogueText);
    }

    void DrawFunctionAction(FunctionNarrative action)
    {
        action.functionID = (FunctionID)EditorGUILayout.EnumPopup("Function ID", action.functionID);
        action.name = EditorGUILayout.TextField("Function Name", action.name);
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
        // 새로운 ScriptableObject 인스턴스 생성

        string path = EditorUtility.SaveFilePanel(
            "Save Narrative Data",
            "Assets/NarrativeData",
            tempFileName != "" ? tempFileName : "narrativeData",
            "json");

        // 사용자가 경로를 선택했는지 확인
        if (!string.IsNullOrEmpty(path))
        {
            string json = JsonConvert.SerializeObject(narrativeData, Formatting.Indented, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All // 타입 정보를 포함시킴
            });

            File.WriteAllText(path, json);
            Debug.Log("Narrative data saved to: " + path);
        }
        else
        {
            Debug.LogWarning("Save operation was canceled or no valid path was provided.");
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

                    tempFileName = tempFileName.Replace("dat", "json");

                    Debug.Log("Data loaded from " + filePath);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("Failed to load data: " + ex.Message);
            }
        }
    }
    // NarrativeData를 JSON 형식으로 저장하는 메서드
    public void SaveNarrativeData_Asset()
    {
        string path = EditorUtility.SaveFilePanel(
            "Save Narrative Data",
            "Assets/NarrativeData",
            tempFileName != "" ? tempFileName : "narrativeData",
            "json");

        // 경로가 유효한지 확인
        if (!string.IsNullOrEmpty(path))
        {
            // NarrativeData를 JSON 형식으로 직렬화
            string json = JsonUtility.ToJson(narrativeData, true);

            // 파일에 JSON 데이터를 기록
            File.WriteAllText(path, json);

            Debug.Log("Narrative data saved to: " + path);
        }
        else
        {
            Debug.LogWarning("Save operation was canceled or no valid path was provided.");
        }
    }

    // JSON 파일로부터 NarrativeData를 로드하는 메서드
    public void LoadNarrativeData_Asset()
    {
        // OpenFilePanel을 사용하여 파일 열기 경로 선택
        string path = EditorUtility.OpenFilePanel("Load Narrative Data", "", "json");

        // 경로가 유효한지 확인
        if (!string.IsNullOrEmpty(path))
        {
            string json = File.ReadAllText(path);
            narrativeData = JsonConvert.DeserializeObject<NarrativeData>(json, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All // 타입 정보를 고려하여 역직렬화
            });

            Debug.Log("Narrative data loaded from: " + path);
        }
        else
        {
            Debug.LogWarning("Load operation was canceled or no valid path was provided.");
        }
    }
}


//    Player = 0,
//    Bartender = 100,
//    Bar_Eleveator,
//    Bar_Shelf,
//    Bar_Desk,
//    Bar_Wall,
//    Boss_1_Room_Center = 200,
//    Boss_1,
//    Boss_1_1,
//    Boss_1_2,
//    Boss_1_3,
//    Boss_1_4,


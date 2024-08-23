using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

[CustomEditor(typeof(StageDataScriptableObject))]
public class StageDataEditor : Editor
{
    private ReorderableList reorderableList;

    private void OnEnable()
    {
        reorderableList = new ReorderableList(serializedObject,
                serializedObject.FindProperty("narrativeDataPaths"),
                true, true, true, true);

        reorderableList.drawHeaderCallback = (Rect rect) =>
        {
            EditorGUI.LabelField(rect, "Narrative Data Paths");
        };

        reorderableList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            var element = reorderableList.serializedProperty.GetArrayElementAtIndex(index);
            rect.y += 2;
            element.stringValue = EditorGUI.TextField(
                new Rect(rect.x, rect.y, rect.width - 60, EditorGUIUtility.singleLineHeight),
                element.stringValue
            );

            // Show preview of the file content
            if (GUI.Button(new Rect(rect.x + rect.width - 60, rect.y, 60, EditorGUIUtility.singleLineHeight), "Preview"))
            {
                string fullPath = Application.dataPath + element.stringValue;

                try
                {
                    using FileStream fileStream = new FileStream(fullPath, FileMode.Open);
                    BinaryFormatter formatter = new BinaryFormatter();
                    NarrativeData narrativeData = (NarrativeData)formatter.Deserialize(fileStream);

                    string narrativeCount = narrativeData.narratives.Count.ToString();

                    EditorUtility.DisplayDialog("Preview", narrativeCount + "개의 블록으로 이루어진 내러티브 파일", "OK");
                }
                catch (System.Exception ex)
                {
                    EditorUtility.DisplayDialog("Error", "File not found: " + fullPath, "OK");
                    Debug.LogError("Failed to load data: " + ex.Message);
                }
            }
        };

        reorderableList.onAddCallback = (ReorderableList list) =>
        {
            string path = EditorUtility.OpenFilePanel("Select Narrative Data File", "", "dat");
            if (!string.IsNullOrEmpty(path))
            {
                string relativePath = path.Substring(Application.dataPath.Length);
                var index = list.serializedProperty.arraySize;
                list.serializedProperty.InsertArrayElementAtIndex(index);
                list.serializedProperty.GetArrayElementAtIndex(index).stringValue = relativePath;
                serializedObject.ApplyModifiedProperties();
            }
        };

        reorderableList.onRemoveCallback = (ReorderableList list) =>
        {
            if (EditorUtility.DisplayDialog("Warning!",
                "Are you sure you want to remove the selected item?", "Yes", "No"))
            {
                ReorderableList.defaultBehaviours.DoRemoveButton(list);
                serializedObject.ApplyModifiedProperties();
            }
        };
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Narrative Data", EditorStyles.boldLabel);

        serializedObject.Update();
        reorderableList.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
    }
}

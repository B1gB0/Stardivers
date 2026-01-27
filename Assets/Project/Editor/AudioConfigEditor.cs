#if UNITY_EDITOR
using System.Collections.Generic;
using Project.Scripts.Audio;
using Project.Scripts.Audio.Sounds;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AudioConfig))]
public class AudioConfigEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        
        EditorGUILayout.LabelField("Addressables Settings", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_soundLabel"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_musicLabel"));
        
        EditorGUILayout.Space(20);
        EditorGUILayout.LabelField("Sounds List", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("Sounds"), true);
        
        serializedObject.ApplyModifiedProperties();
        
        EditorGUILayout.Space(20);
        EditorGUILayout.LabelField("Actions", EditorStyles.boldLabel);
        
        EditorGUILayout.BeginHorizontal();
        
        if (GUILayout.Button("Import from Addressables", GUILayout.Height(30)))
        {
            var config = (AudioConfig)target;
            config.ImportFromAddressables();
        }
        
        if (GUILayout.Button("Clear All", GUILayout.Width(100), GUILayout.Height(30)))
        {
            if (EditorUtility.DisplayDialog(
                "Clear All", 
                "Удалить все звуки из конфига?", 
                "Да", "Нет"))
            {
                var config = (AudioConfig)target;
                Undo.RecordObject(config, "Clear All");
                config.Sounds.Clear();
                EditorUtility.SetDirty(config);
            }
        }
        
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.Space(10);
        
        if (GUILayout.Button("Generate Sounds Enum", GUILayout.Height(30)))
        {
            SoundsEnumGenerator.Generate((AudioConfig)target);
        }
        
        EditorGUILayout.Space(10);
        
        EditorGUILayout.LabelField("Quick Actions", EditorStyles.boldLabel);
        
        if (GUILayout.Button("Sort by Name"))
        {
            var config = (AudioConfig)target;
            Undo.RecordObject(config, "Sort by Name");
            
            config.Sounds.Sort((a, b) => string.Compare(a.ClipName, b.ClipName));
            
            EditorUtility.SetDirty(config);
        }
        
        if (GUILayout.Button("Validate"))
        {
            ValidateConfig((AudioConfig)target);
        }
    }
    
    private void ValidateConfig(AudioConfig config)
    {
        bool hasErrors = false;
        var clipNames = new HashSet<string>();
        
        foreach (var sound in config.Sounds)
        {
            if (string.IsNullOrEmpty(sound.ClipName))
            {
                Debug.LogError($"Sound has empty ClipName!");
                hasErrors = true;
            }
            else if (!clipNames.Add(sound.ClipName))
            {
                Debug.LogError($"Duplicate ClipName found: {sound.ClipName}");
                hasErrors = true;
            }
        }
        
        if (!hasErrors)
        {
            Debug.Log($"AudioConfig is valid. Total sounds: {config.Sounds.Count}");
        }
    }
}
#endif
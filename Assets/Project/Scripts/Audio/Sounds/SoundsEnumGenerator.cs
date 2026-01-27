#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Project.Scripts.Audio.Sounds
{
    public static class SoundsEnumGenerator
    {
        private const string EnumTemplate = @"namespace Project.Scripts.Audio.Sounds
{{
    public enum SoundsType
    {{
        None,
{0}
    }}
}}";

        [MenuItem("Tools/Audio/Generate Sounds Enum")]
        public static void GenerateAll()
        {
            var guids = AssetDatabase.FindAssets("t:AudioConfig");

            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var config = AssetDatabase.LoadAssetAtPath<AudioConfig>(path);

                if (config != null)
                {
                    Generate(config);
                }
            }

            Debug.Log("All AudioConfig enums generated!");
        }

        public static void Generate(AudioConfig config)
        {
            if (config == null)
            {
                Debug.LogError("AudioConfig is null!");
                return;
            }

            var allClipNames = new HashSet<string>();
            var stringBuilder = new StringBuilder();

            foreach (var sound in config.Sounds)
            {
                if (!string.IsNullOrEmpty(sound.ClipName))
                {
                    allClipNames.Add(sound.ClipName);
                }
            }

            bool isFirst = true;
            foreach (var clipName in allClipNames)
            {
                var enumName = SanitizeIdentifier(clipName);

                if (!isFirst)
                {
                    stringBuilder.AppendLine();
                }

                stringBuilder.Append($"        {enumName},");
                isFirst = false;
            }

            var content = string.Format(EnumTemplate, stringBuilder);
            var path = Path.Combine(Application.dataPath, "Project/Scripts/Audio/Sounds/SoundsType.cs");

            Directory.CreateDirectory(Path.GetDirectoryName(path));
            File.WriteAllText(path, content);

            AssetDatabase.Refresh();
            Debug.Log($"Sounds enum generated with {allClipNames.Count} entries");
        }

        private static string SanitizeIdentifier(string id)
        {
            if (string.IsNullOrEmpty(id))
                return "InvalidSound";

            var result = new StringBuilder();
            bool lastWasUnderscore = false;

            foreach (char c in id)
            {
                if (char.IsLetterOrDigit(c))
                {
                    result.Append(c);
                    lastWasUnderscore = false;
                }
                else if (c == '_' || c == ' ')
                {
                    if (!lastWasUnderscore && result.Length > 0)
                    {
                        result.Append('_');
                        lastWasUnderscore = true;
                    }
                }
            }

            if (result.Length > 0 && result[result.Length - 1] == '_')
            {
                result.Length--;
            }

            var sanitized = result.ToString();

            if (sanitized.Length > 0 && char.IsDigit(sanitized[0]))
            {
                sanitized = "_" + sanitized;
            }

            if (string.IsNullOrEmpty(sanitized))
            {
                sanitized = "InvalidSound";
            }

            return sanitized;
        }
    }
}
#endif
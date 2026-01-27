using System.Collections.Generic;
using System.Linq;
using Project.Scripts.Audio.Sounds;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;

namespace Project.Scripts.Audio
{
    [CreateAssetMenu(menuName = "AudioConfig")]
    public class AudioConfig : ScriptableObject
    {
        public List<Sound> Sounds = new();

#if UNITY_EDITOR
        [SerializeField] private string _soundLabel = "Sound";
        [SerializeField] private string _musicLabel = "Music";

        public void ImportFromAddressables()
        {
            var settings = AddressableAssetSettingsDefaultObject.Settings;
            if (settings == null)
            {
                Debug.LogError("Addressable settings not found!");
                return;
            }

            Undo.RecordObject(this, "Import from Addressables");
            Sounds.Clear();

            ImportByLabel(_soundLabel);
            ImportByLabel(_musicLabel);

            EditorUtility.SetDirty(this);
            Debug.Log($"Imported from Addressables: {Sounds.Count} sounds total");
        }

        private void ImportByLabel(string label)
        {
            if (string.IsNullOrEmpty(label)) return;

            var settings = AddressableAssetSettingsDefaultObject.Settings;
            var entries = new List<AddressableAssetEntry>();
            settings.GetAllAssets(entries, false);

            foreach (var entry in entries)
            {
                if (entry.labels.Contains(label))
                {
                    var clip = AssetDatabase.LoadAssetAtPath<AudioClip>(entry.AssetPath);
                    if (clip != null)
                    {
                        var existingSound = Sounds.FirstOrDefault(s => s.ClipName == clip.name);
                        if (existingSound == null)
                        {
                            var sound = new Sound
                            {
                                ClipName = clip.name,
                                Volume = 1f,
                            };

                            if (label == _musicLabel)
                                sound.IsLoop = true;

                            Sounds.Add(sound);
                        }
                    }
                }
            }
        }
#endif
    }
}
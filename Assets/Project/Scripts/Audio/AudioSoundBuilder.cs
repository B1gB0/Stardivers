using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Project.Scripts.Audio.Sounds;
using Project.Scripts.Services;
using UnityEngine;

namespace Project.Scripts.Audio
{
    public class AudioSoundBuilder
    {
        private readonly IResourceService _resourceService;
        private readonly Dictionary<SoundsType, string> _soundPaths = new();

        public AudioSoundBuilder(IResourceService resourceService)
        {
            _resourceService = resourceService;
        }

        public AudioSoundBuilder AddSound(SoundsType type, string path)
        {
            _soundPaths[type] = path;
            return this;
        }

        public async UniTask<Dictionary<SoundsType, Sound>> Build()
        {
            var dictionary = new Dictionary<SoundsType, Sound>();
            var loadTasks = new List<UniTask>();

            foreach (var (soundType, path) in _soundPaths)
            {
                var loadTask = LoadAndAddSound(dictionary, soundType, path);
                loadTasks.Add(loadTask);
            }

            await UniTask.WhenAll(loadTasks);
            return dictionary;
        }

        private async UniTask LoadAndAddSound(Dictionary<SoundsType, Sound> dictionary, SoundsType type, string path)
        {
            try
            {
                var sound = await _resourceService.Load<Sound>(path);
                if (sound != null)
                {
                    dictionary[type] = sound;
                }
                else
                {
                    Debug.LogError($"Failed to load sound: {path}");
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error loading sound {path}: {ex.Message}");
            }
        }
    }
}
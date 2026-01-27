using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Project.Scripts.Services;
using UnityEngine;

namespace Project.Scripts
{
    public class GenericScriptableObjectBuilder<TKey, TValue>
    {
        private readonly IResourceService _resourceService;
        private readonly Dictionary<TKey, string> _resourcePaths = new();
        private readonly string _nameOfBuilder;

        protected GenericScriptableObjectBuilder(IResourceService resourceService, string nameOfBuilder = "resource")
        {
            _resourceService = resourceService;
            _nameOfBuilder = nameOfBuilder;
        }

        public GenericScriptableObjectBuilder<TKey, TValue> AddScriptableObject(TKey type, string path)
        {
            _resourcePaths[type] = path;
            return this;
        }

        public async UniTask<Dictionary<TKey, TValue>> Build()
        {
            var dictionary = new Dictionary<TKey, TValue>();
            var loadTasks = new List<UniTask>();

            foreach (var (resourceType, path) in _resourcePaths)
            {
                var loadTask = LoadAndAddResource(dictionary, resourceType, path);
                loadTasks.Add(loadTask);
            }

            await UniTask.WhenAll(loadTasks);
            return dictionary;
        }

        private async UniTask LoadAndAddResource(Dictionary<TKey, TValue> dictionary, TKey type, string path)
        {
            try
            {
                var resource = await _resourceService.Load<TValue>(path);
                if (resource != null)
                {
                    dictionary[type] = resource;
                }
                else
                {
                    Debug.LogError($"Failed to load {_nameOfBuilder}: {path}");
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error loading {_nameOfBuilder} {path}: {ex.Message}");
            }
        }
    }
}
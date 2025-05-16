using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Project.Scripts.Services
{
    public class ResourceService : IResourceService
    {
        public async UniTask<T> Load<T>(string assetName)
        {
            try
            {
                return await Addressables.LoadAssetAsync<T>(assetName);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                Debug.LogError("Cant load " + assetName);
            }

            return default;
        }
    }
}
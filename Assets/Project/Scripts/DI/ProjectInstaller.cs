using Project.Scripts.DataBase;
using Project.Scripts.Services;
using UnityEngine;
using Reflex;
using Reflex.Core;

namespace Project.Scripts.DI
{
    public class ProjectInstaller : MonoBehaviour, IInstaller
    {
        [SerializeField] private AudioSoundsService _audioSoundsServicePrefab;
        [SerializeField] private OperationService operationServicePrefab;
        
        public void InstallBindings(ContainerBuilder builder)
        {
            builder.AddSingleton(typeof(DataBaseService), typeof(IDataBaseService));
            builder.AddSingleton( typeof(PauseService), typeof(IPauseService));

            AudioSoundsService audioSoundsService = Instantiate(_audioSoundsServicePrefab);
            OperationService operationService = Instantiate(operationServicePrefab);
            
            builder.AddSingleton(audioSoundsService);
            builder.AddSingleton(operationService);
            
            DontDestroyOnLoad(audioSoundsService.gameObject);
            DontDestroyOnLoad(operationService); ;
        }
    }
}

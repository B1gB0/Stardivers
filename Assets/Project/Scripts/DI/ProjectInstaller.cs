using Project.Scripts.Services;
using Project.Scripts.UI;
using Project.Scripts.UI.StateMachine;
using Reflex.Core;
using UnityEngine;

namespace Project.Scripts.DI
{
    public class ProjectInstaller : MonoBehaviour, IInstaller
    {
        [SerializeField] private AudioSoundsService _audioSoundsServicePrefab;
        [SerializeField] private OperationSetterService operationSetterServicePrefab;
        
        public void InstallBindings(ContainerBuilder containerBuilder)
        {
            PauseService pauseService = new PauseService();

            AudioSoundsService audioSoundsService = Instantiate(_audioSoundsServicePrefab);
            OperationSetterService operationSetterService = Instantiate(operationSetterServicePrefab);
            
            containerBuilder.AddSingleton(pauseService);
            containerBuilder.AddSingleton(audioSoundsService);
            containerBuilder.AddSingleton(operationSetterService);
            
            DontDestroyOnLoad(audioSoundsService.gameObject);
            DontDestroyOnLoad(operationSetterService);
        }
    }
}

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
        [SerializeField] private OperationAndLevelSetterService operationAndLevelSetterServicePrefab;
        
        public void InstallBindings(ContainerBuilder containerBuilder)
        {
            PauseService pauseService = new PauseService();

            AudioSoundsService audioSoundsService = Instantiate(_audioSoundsServicePrefab);
            OperationAndLevelSetterService operationAndLevelSetterService = Instantiate(operationAndLevelSetterServicePrefab);
            
            containerBuilder.AddSingleton(pauseService);
            containerBuilder.AddSingleton(audioSoundsService);
            containerBuilder.AddSingleton(operationAndLevelSetterService);
            
            DontDestroyOnLoad(audioSoundsService.gameObject);
            DontDestroyOnLoad(operationAndLevelSetterService);
        }
    }
}

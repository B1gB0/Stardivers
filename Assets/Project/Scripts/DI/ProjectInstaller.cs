using Project.Scripts.UI;
using Project.Scripts.UI.StateMachine;
using Reflex.Core;
using UnityEngine;

namespace Project.Scripts.DI
{
    public class ProjectInstaller : MonoBehaviour, IInstaller
    {
        [SerializeField] private AudioSoundsService _audioSoundsServicePrefab;
        
        public void InstallBindings(ContainerBuilder containerBuilder)
        {
            PauseService pauseService = new PauseService();

            AudioSoundsService audioSoundsService = Instantiate(_audioSoundsServicePrefab);
            
            containerBuilder.AddSingleton(pauseService);
            containerBuilder.AddSingleton(audioSoundsService);
            
            DontDestroyOnLoad(audioSoundsService.gameObject);
        }
    }
}

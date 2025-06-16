using System.Collections.Generic;
using Project.Scripts.Game.GameRoot;
using Project.Scripts.Services;
using UnityEngine;
using Reflex.Core;
using Reflex.Injectors;
using Unity.VisualScripting;

namespace Project.Scripts.DI
{
    public class ProjectInstaller : MonoBehaviour, IInstaller
    {
        [SerializeField] private AudioSoundsService _audioSoundsServicePrefab;
        [SerializeField] private OperationService _operationServicePrefab;
        [SerializeField] private UIRootView _uiRootViewPrefab;
        [SerializeField] private GameEntryPoint _gameEntryPointPrefab;
        
        private readonly List<object> _monoServices = new ();
        private readonly List<GameObject> _monoServiceObjects = new ();
        
        public void InstallBindings(ContainerBuilder builder)
        {
            RegisterCoreServices(builder);
            CreateMonoServices();
            RegisterCreatedServices(builder);
            RegisterContainerDependentServices(builder);
        }
        
        private void RegisterCoreServices(ContainerBuilder builder)
        {
            builder.AddSingleton(typeof(ResourceService), typeof(IResourceService));
            builder.AddSingleton(typeof(DataBaseService), typeof(IDataBaseService));
            builder.AddSingleton(typeof(PauseService), typeof(IPauseService));
            builder.AddSingleton(typeof(FloatingTextService), typeof(IFloatingTextService));
        }
        
        private void CreateMonoServices()
        {
            var serviceParent = new GameObject("Services");
            DontDestroyOnLoad(serviceParent);
            
            CreateService(_audioSoundsServicePrefab, serviceParent);
            CreateService(_operationServicePrefab, serviceParent);
            CreateService(_uiRootViewPrefab, serviceParent);
            CreateService(_gameEntryPointPrefab, serviceParent);
        }

        private void CreateService<T>(T prefab, GameObject parent) where T : MonoBehaviour
        {
            var instance = Instantiate(prefab, parent.transform);
            _monoServices.Add(instance);
            _monoServiceObjects.Add(instance.gameObject);
            DontDestroyOnLoad(instance.gameObject);
        }
        
        private void RegisterCreatedServices(ContainerBuilder builder)
        {
            foreach (var service in _monoServices)
            {
                builder.AddSingleton(service);
                
                var serviceType = service.GetType();
                var interfaces = serviceType.GetInterfaces();
                
                foreach (var interfaceType in interfaces)
                {
                    builder.AddSingleton(serviceType, interfaceType);
                }
            }
        }

        private void RegisterContainerDependentServices(ContainerBuilder builder)
        {
            builder.OnContainerBuilt += container =>
            {
                foreach (var service in _monoServiceObjects)
                {
                    GameObjectInjector.InjectObject(service, container);
                }
                
                foreach (var service in _monoServices)
                {
                    if (service is IInitializable initializable)
                    {
                        initializable.Initialize();
                    }
                }
            };
        }
        
        private void OnDestroy()
        {
            foreach (var obj in _monoServiceObjects)
            {
                if (obj != null) Destroy(obj);
            }
        }
    }
}

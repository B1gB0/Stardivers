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
            Debug.Log("Регистрация сервисов");
            
            RegisterCoreServices(builder);
            Debug.Log("Перед созданием моно сервисов");
            CreateMonoServices();
            RegisterCreatedServices(builder);
            RegisterContainerDependentServices(builder);
            Debug.Log("Конец InstallBindings");
        }
        
        private void RegisterCoreServices(ContainerBuilder builder)
        {
            builder.AddSingleton(typeof(ResourceService), typeof(IResourceService));
            builder.AddSingleton(typeof(DataBaseService), typeof(IDataBaseService));
            builder.AddSingleton(typeof(PauseService), typeof(IPauseService));
            builder.AddSingleton(typeof(FloatingTextService), typeof(IFloatingTextService));
            builder.AddSingleton(typeof(CharacteristicsWeaponDataService), 
                typeof(ICharacteristicsWeaponDataService));
            builder.AddSingleton(typeof(CardService), typeof(ICardService));
            builder.AddSingleton(typeof(EnemyService), typeof(IEnemyService));
            builder.AddSingleton(typeof(PlayerService), typeof(IPlayerService));
            builder.AddSingleton(typeof(GoldService), typeof(IGoldService));
            Debug.Log("Регистрация сервисов завершена");
        }
        
        private void CreateMonoServices()
        {
            Debug.Log("Создание моно сервисов");

            CreateService(_audioSoundsServicePrefab);
            CreateService(_operationServicePrefab);
            CreateService(_uiRootViewPrefab);
            CreateService(_gameEntryPointPrefab);
        }

        private void CreateService<T>(T prefab) where T : MonoBehaviour
        {
            Debug.Log($"Создание {prefab.name}");
            var instance = Instantiate(prefab);
            _monoServices.Add(instance);
            _monoServiceObjects.Add(instance.gameObject);
            DontDestroyOnLoad(instance);
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

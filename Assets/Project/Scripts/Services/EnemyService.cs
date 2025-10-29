using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Project.Scripts.DataBase.Data;
using Project.Scripts.ECS.EntityActors;
using Reflex.Attributes;

namespace Project.Scripts.Services
{
    public class EnemyService : IService, IEnemyService
    {
        private readonly Dictionary<EnemyActorType, EnemyData> _enemiesData = new();
        
        private IDataBaseService _dataBaseService;
        
        public bool IsInitiated { get; private set; }
        
        [Inject]
        public void Construct(IDataBaseService dataBaseService)
        {
            _dataBaseService = dataBaseService;
        }

        public UniTask Init()
        {
            if(IsInitiated)
                return UniTask.CompletedTask;
            
            foreach (var enemy in _dataBaseService.Content.Enemies)
            {
                _enemiesData.TryAdd(enemy.Type, enemy);
            }

            IsInitiated = true;
            
            return UniTask.CompletedTask;
        }

        public EnemyData GetEnemyDataByType(EnemyActorType type)
        {
            return _enemiesData[type];
        }
    }
}
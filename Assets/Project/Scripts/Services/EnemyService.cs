using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Project.Scripts.DataBase.Data;
using Project.Scripts.ECS.EntityActors;
using Reflex.Attributes;

namespace Project.Scripts.Services
{
    public class EnemyService : Service, IEnemyService
    {
        private readonly Dictionary<EnemyAlienActorType, EnemyData> _enemiesData = new();
        
        private IDataBaseService _dataBaseService;
        
        [Inject]
        public void Construct(IDataBaseService dataBaseService)
        {
            _dataBaseService = dataBaseService;
        }

        public override UniTask Init()
        {
            foreach (var enemy in _dataBaseService.Content.Enemies)
            {
                _enemiesData.TryAdd(enemy.Type, enemy);
            }
            
            return UniTask.CompletedTask;
        }

        public EnemyData GetEnemyDataByType(EnemyAlienActorType type)
        {
            return _enemiesData[type];
        }
    }
}
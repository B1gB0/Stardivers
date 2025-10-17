using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Project.Scripts.DataBase.Data;
using Project.Scripts.ECS.EntityActors;
using Reflex.Attributes;

namespace Project.Scripts.Services
{
    public class CoreService : Service, ICoreService
    {
        private readonly Dictionary<CoreType, CoreData> _coresData = new();
        
        private IDataBaseService _dataBaseService;
        
        [Inject]
        public void Construct(IDataBaseService dataBaseService)
        {
            _dataBaseService = dataBaseService;
        }

        public override UniTask Init()
        {
            foreach (var core in _dataBaseService.Content.Cores)
            {
                _coresData.TryAdd(core.Type, core);
            }
            
            return UniTask.CompletedTask;
        }

        public CoreData GetCoreDataByType(CoreType type)
        {
            return _coresData[type];
        }
    }
}
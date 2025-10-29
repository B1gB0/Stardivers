using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Project.Scripts.DataBase.Data;
using Project.Scripts.ECS.EntityActors;
using Reflex.Attributes;

namespace Project.Scripts.Services
{
    public class CoreService : IService, ICoreService
    {
        private readonly Dictionary<CoreType, CoreData> _coresData = new();
        
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
            
            foreach (var core in _dataBaseService.Content.Cores)
            {
                _coresData.TryAdd(core.Type, core);
            }

            IsInitiated = true;
            
            return UniTask.CompletedTask;
        }

        public CoreData GetCoreDataByType(CoreType type)
        {
            return _coresData[type];
        }
    }
}
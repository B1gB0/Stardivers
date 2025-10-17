using Cysharp.Threading.Tasks;
using Project.Scripts.DataBase.Data;
using Project.Scripts.ECS.EntityActors;

namespace Project.Scripts.Services
{
    public interface ICoreService
    {
        public UniTask Init();
        public CoreData GetCoreDataByType(CoreType type);
    }
}
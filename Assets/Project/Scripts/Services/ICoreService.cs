using Project.Scripts.DataBase.Data;
using Project.Scripts.ECS.EntityActors;

namespace Project.Scripts.Services
{
    public interface ICoreService : IService
    {
        public CoreData GetCoreDataByType(CoreType type);
    }
}
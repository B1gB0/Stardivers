using Project.Scripts.DataBase.Data;
using Project.Scripts.ECS.EntityActors;

namespace Project.Scripts.Services
{
    public interface IEnemyService : IService
    {
        public EnemyData GetEnemyDataByType(EnemyActorType type);
    }
}
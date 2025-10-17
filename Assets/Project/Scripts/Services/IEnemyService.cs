using Cysharp.Threading.Tasks;
using Project.Scripts.DataBase.Data;
using Project.Scripts.ECS.EntityActors;

namespace Project.Scripts.Services
{
    public interface IEnemyService
    {
        public UniTask Init();
        public EnemyData GetEnemyDataByType(EnemyActorType type);
    }
}
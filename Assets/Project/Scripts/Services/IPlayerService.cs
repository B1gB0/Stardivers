using Cysharp.Threading.Tasks;
using Project.Scripts.DataBase.Data;
using Project.Scripts.ECS.EntityActors;

namespace Project.Scripts.Services
{
    public interface IPlayerService
    {
        public UniTask Init();
        public PlayerData GetPlayerByType(PlayerActorType type);
    }
}
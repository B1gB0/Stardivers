using Project.Scripts.DataBase.Data;
using Project.Scripts.ECS.Components;
using Project.Scripts.ECS.EntityActors;

namespace Project.Scripts.Services
{
    public interface IPlayerService : IService
    {
        public PlayerActor PlayerActor { get; }
        public PlayerMovableComponent PlayerMovableComponent { get; }
        public PlayerData GetPlayerDataByType(PlayerActorType type);
        public void GetPlayer(PlayerActor playerActor, PlayerMovableComponent playerMovableComponent);
        public void ChangePlayerMovableComponent(PlayerMovableComponent newMovableComponent);
    }
}
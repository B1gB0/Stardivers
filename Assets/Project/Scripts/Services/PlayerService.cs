using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Project.Scripts.DataBase.Data;
using Project.Scripts.ECS.Components;
using Project.Scripts.ECS.EntityActors;
using Reflex.Attributes;

namespace Project.Scripts.Services
{
    public class PlayerService : Service, IPlayerService
    {
        private readonly Dictionary<PlayerActorType, PlayerData> _playersData = new();
        
        private IDataBaseService _dataBaseService;

        [Inject]
        public void Construct(IDataBaseService dataBaseService)
        {
            _dataBaseService = dataBaseService;
        }
        
        public PlayerActor PlayerActor { get; private set; }
        public PlayerMovableComponent PlayerMovableComponent { get; private set; }

        public override UniTask Init()
        {
            foreach (var player in _dataBaseService.Content.Players)
            {
                _playersData.TryAdd(player.Type, player);
            }
            
            return UniTask.CompletedTask;
        }

        public PlayerData GetPlayerDataByType(PlayerActorType type)
        {
            return _playersData[type];
        }

        public void GetPlayer(PlayerActor playerActor, PlayerMovableComponent playerMovableComponent)
        {
            PlayerActor = playerActor;
            PlayerMovableComponent = playerMovableComponent;
        }

        public void ChangePlayerMovableComponent(PlayerMovableComponent newMovableComponent)
        {
            PlayerMovableComponent = newMovableComponent;
        }
    }
}
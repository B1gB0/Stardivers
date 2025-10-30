using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Project.Scripts.DataBase.Data;
using Project.Scripts.ECS.Components;
using Project.Scripts.ECS.EntityActors;
using Reflex.Attributes;

namespace Project.Scripts.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly Dictionary<PlayerActorType, PlayerData> _playersData = new();
        private readonly List<int> _playerLevels= new();
        
        private IDataBaseService _dataBaseService;
        
        public bool IsInitiated { get; private set; }

        [Inject]
        public void Construct(IDataBaseService dataBaseService)
        {
            _dataBaseService = dataBaseService;
        }
        
        public PlayerActor PlayerActor { get; private set; }
        public PlayerMovableComponent PlayerMovableComponent { get; private set; }

        public UniTask Init()
        {
            if(IsInitiated)
                return UniTask.CompletedTask;
            
            foreach (var player in _dataBaseService.Content.Players)
            {
                _playersData.TryAdd(player.Type, player);
            }

            foreach (var playerLevel in _dataBaseService.Content.PlayerLevels)
            {
                _playerLevels.Add(playerLevel.RequiredExperience);
            }

            IsInitiated = true;
            
            return UniTask.CompletedTask;
        }

        public PlayerData GetPlayerDataByType(PlayerActorType type)
        {
            return _playersData[type];
        }

        public List<int> GetPlayerLevels()
        {
            return _playerLevels;
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
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Leopotam.Ecs;
using Project.Scripts.DataBase.Data;
using Project.Scripts.ECS.Components;
using Project.Scripts.ECS.EntityActors;
using Project.Scripts.Weapon.CharacteristicsOfWeapon;
using Reflex.Attributes;
using UnityEngine;
using YG;

namespace Project.Scripts.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly Dictionary<PlayerActorType, PlayerData> _playersData = new();
        private readonly List<int> _playerLevels = new();

        private IDataBaseService _dataBaseService;

        public bool IsInitiated { get; private set; }

        [Inject]
        public void Construct(IDataBaseService dataBaseService)
        {
            _dataBaseService = dataBaseService;
        }

        public PlayerActor PlayerActor { get; private set; }
        public EcsEntity PlayerEntity { get; private set; }

        public UniTask Init()
        {
            if (IsInitiated)
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

        public void GetPlayer(PlayerActor playerActor, EcsEntity playerEntity)
        {
            PlayerActor = playerActor;
            PlayerEntity = playerEntity;
        }

        public PlayerCharacteristics InitPlayerCharacteristics()
        {
            var characteristics = YG2.saves.PlayerCharacteristics;

            if (characteristics != null)
                characteristics.SetCharacteristics();
            else
            {
                characteristics = new PlayerCharacteristics(this);
                characteristics.SetStartingCharacteristics(GetPlayerDataByType(PlayerActorType.CommonStardiver));
            }

            YG2.saves.PlayerCharacteristics = characteristics;

            return characteristics;
        }

        public void ChangeMoveSpeed(float moveSpeed)
        {
            ref var movableComponent = ref PlayerEntity.Get<PlayerMovableComponent>();
            movableComponent.MoveSpeed = moveSpeed;
        }
    }
}
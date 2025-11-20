using System.Collections.Generic;
using Leopotam.Ecs;
using Project.Scripts.DataBase.Data;
using Project.Scripts.ECS.EntityActors;
using Project.Scripts.Weapon.CharacteristicsOfWeapon;

namespace Project.Scripts.Services
{
    public interface IPlayerService : IService
    {
        public PlayerActor PlayerActor { get; }
        public PlayerData GetPlayerDataByType(PlayerActorType type);
        public bool CheckMoveSystem();
        public List<int> GetPlayerLevels();
        public void GetPlayer(PlayerActor playerActor, EcsEntity playerEntity);
        public void ChangeMoveSpeed(float moveSpeed);
        public PlayerCharacteristics InitPlayerCharacteristics();
        public void AddHealthByFactor(float healthValue);
    }
}
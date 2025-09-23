using Project.Scripts.DataBase.Data;
using Project.Scripts.ECS.Components;
using Project.Scripts.ECS.EntityActors;
using Project.Scripts.Services;
using UnityEngine;

namespace Project.Scripts.Weapon.CharacteristicsOfWeapon
{
    public class PlayerCharacteristics : Characteristics
    {
        private readonly IPlayerService _playerService;

        public PlayerCharacteristics(IPlayerService playerService)
        {
            _playerService = playerService;
        }

        public override void ApplyImprovement(CharacteristicType type, float factor)
        {
            switch (type)
            {
                case CharacteristicType.Health:
                    IncreaseHealth(factor);
                    break;
                case CharacteristicType.DiggingSpeed:
                    IncreaseDiggingSpeed(factor);
                    break;
                case CharacteristicType.MoveSpeed:
                    IncreaseMoveSpeed(factor);
                    break;
            }
        }

        protected override void IncreaseHealth(float healthValue)
        {
            _playerService.PlayerActor.Health.ImproveHealth(healthValue);
        }

        protected override void IncreaseDiggingSpeed(float diggingSpeedFactor)
        {
            PlayerData data = _playerService.GetPlayerDataByType(PlayerActorType.CommonStardiver);

            float newDiggingSpeed = data.DiggingSpeed - Mathf.Round(data.DiggingSpeed * diggingSpeedFactor);

            _playerService.PlayerActor.MiningToolActor.ChangeDiggingSpeed(newDiggingSpeed);
        }

        protected override void IncreaseMoveSpeed(float moveSpeedFactor)
        {
            PlayerData data = _playerService.GetPlayerDataByType(PlayerActorType.CommonStardiver);
            
            float newMoveSpeed = data.MoveSpeed + Mathf.Round(data.MoveSpeed * moveSpeedFactor);

            PlayerMovableComponent newMovableComponent = _playerService.PlayerMovableComponent;
            newMovableComponent.MoveSpeed = newMoveSpeed;

            _playerService.ChangePlayerMovableComponent(newMovableComponent);
        }
    }
}
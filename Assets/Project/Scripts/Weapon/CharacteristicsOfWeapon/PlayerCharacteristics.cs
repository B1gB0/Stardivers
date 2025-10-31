using Project.Scripts.DataBase.Data;
using Project.Scripts.ECS.Components;
using Project.Scripts.ECS.EntityActors;
using Project.Scripts.Services;
using UnityEngine;

namespace Project.Scripts.Weapon.CharacteristicsOfWeapon
{
    public class PlayerCharacteristics
    {
        private readonly IPlayerService _playerService;
        
        private float _health;
        private float _diggingSpeed;
        private float _moveSpeed;
        
        public float Health => _health;
        public float DiggingSpeed => _diggingSpeed;
        public float MoveSpeed => _moveSpeed;

        public PlayerCharacteristics(IPlayerService playerService)
        {
            _playerService = playerService;
        }
        
        public void SetStartingCharacteristics(PlayerData data)
        {
            _health = data.Health;
            _diggingSpeed = data.DiggingSpeed;
            _moveSpeed = data.MoveSpeed;
            
            SetCharacteristics();
        }

        public void SetCharacteristics()
        {
            _playerService.PlayerActor.Health.SetNewMaxHealth(_health);
            _playerService.PlayerActor.MiningToolActor.ChangeDiggingSpeed(_diggingSpeed);
            SetMovableComponentSpeed(_moveSpeed);
        }

        public void ApplyImprovement(CharacteristicType type, float factor)
        {
            switch (type)
            {
                case CharacteristicType.Health:
                    IncreaseHealth(factor);
                    break;
                case CharacteristicType.DiggingSpeed:
                    IncreaseDiggingSpeedFactor(factor);
                    break;
                case CharacteristicType.MoveSpeed:
                    IncreaseMoveSpeed(factor);
                    break;
            }
        }

        private void SetHealth(float healthValue)
        {
            _health += healthValue;
            _playerService.PlayerActor.Health.ImproveHealth(healthValue);
        }

        private void SetDiggingSpeed(float diggingSpeedFactor)
        {
            PlayerData data = _playerService.GetPlayerDataByType(PlayerActorType.CommonStardiver);

            float newDiggingSpeed = data.DiggingSpeed - Mathf.Round(data.DiggingSpeed * diggingSpeedFactor);
            _diggingSpeed = newDiggingSpeed;

            _playerService.PlayerActor.MiningToolActor.ChangeDiggingSpeed(newDiggingSpeed);
        }

        private void SetMoveSpeed(float moveSpeedFactor)
        {
            PlayerData data = _playerService.GetPlayerDataByType(PlayerActorType.CommonStardiver);
            
            float newMoveSpeed = data.MoveSpeed + Mathf.Round(data.MoveSpeed * moveSpeedFactor);

            SetMovableComponentSpeed(newMoveSpeed);
        }

        private void SetMovableComponentSpeed(float newMoveSpeed)
        {
            PlayerMovableComponent newMovableComponent = _playerService.PlayerMovableComponent;
            newMovableComponent.MoveSpeed = newMoveSpeed;
            _moveSpeed = newMoveSpeed;

            _playerService.ChangePlayerMovableComponent(newMovableComponent);
        }

        private void IncreaseHealth(float healthValue)
        {
            SetHealth(healthValue);
        }

        private void IncreaseDiggingSpeedFactor(float diggingSpeedFactor)
        {
            SetDiggingSpeed(diggingSpeedFactor);
        }
        
        private void IncreaseMoveSpeed(float moveSpeedFactor)
        {
            SetMoveSpeed(moveSpeedFactor);
        }
    }
}
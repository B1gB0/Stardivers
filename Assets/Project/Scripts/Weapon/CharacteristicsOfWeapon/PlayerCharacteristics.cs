using Project.Scripts.DataBase.Data;
using Project.Scripts.ECS.EntityActors;
using Project.Scripts.Services;

namespace Project.Scripts.Weapon.CharacteristicsOfWeapon
{
    public class PlayerCharacteristics
    {
        private readonly IPlayerService _playerService;
        
        private float _maxHealth;
        private float _currentHealth;
        private float _diggingSpeed;
        private float _moveSpeed;

        public PlayerCharacteristics(IPlayerService playerService)
        {
            _playerService = playerService;
        }

        public void SetStartingCharacteristics(PlayerData data)
        {
            _maxHealth = data.Health;
            _currentHealth = data.Health;
            _diggingSpeed = data.DiggingSpeed;
            _moveSpeed = data.MoveSpeed;
            SetCharacteristics();
        }

        public void SetCharacteristics()
        {
            _playerService.PlayerActor.Health.LoadHealth(_maxHealth, _currentHealth);
            _playerService.PlayerActor.MiningToolActor.ChangeDiggingSpeed(_diggingSpeed);
            ChangeMovableComponentSpeed(_moveSpeed);
        }

        public void SaveCurrentHealth(float currentHealth)
        {
            _currentHealth = currentHealth;
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
            _maxHealth += healthValue;
            _playerService.PlayerActor.Health.ImproveHealth(healthValue);
        }

        private void SetDiggingSpeed(float diggingSpeedFactor)
        {
            PlayerData data = _playerService.GetPlayerDataByType(PlayerActorType.CommonStardiver);

            float newDiggingSpeed = data.DiggingSpeed - data.DiggingSpeed * diggingSpeedFactor;
            _diggingSpeed = newDiggingSpeed;

            _playerService.PlayerActor.MiningToolActor.ChangeDiggingSpeed(newDiggingSpeed);
        }

        private void SetMoveSpeed(float moveSpeedFactor)
        {
            PlayerData data = _playerService.GetPlayerDataByType(PlayerActorType.CommonStardiver);
            
            float newMoveSpeed = data.MoveSpeed + data.MoveSpeed * moveSpeedFactor;

            ChangeMovableComponentSpeed(newMoveSpeed);
        }

        private void ChangeMovableComponentSpeed(float newMoveSpeed)
        {
            _moveSpeed = newMoveSpeed;
            _playerService.ChangeMoveSpeed(_moveSpeed);
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
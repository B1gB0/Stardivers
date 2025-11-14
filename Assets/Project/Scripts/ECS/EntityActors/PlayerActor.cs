using Project.Scripts.Player.PlayerInputModule;
using Project.Scripts.Weapon.CharacteristicsOfWeapon;
using Project.Scripts.Weapon.Improvements;
using UnityEngine;

namespace Project.Scripts.ECS.EntityActors
{
    public class PlayerActor : EntityActor
    {
        [field: SerializeField] public Rigidbody Rigidbody { get; private set; }
        [field: SerializeField] public PlayerInputController PlayerInputController { get; private set; }
        [field: SerializeField] public MiningToolActor MiningToolActor { get; private set; }
        
        public PlayerCharacteristics PlayerCharacteristics { get; private set; }
        public bool CanFollow { get; private set; }

        public void GetCharacteristics(PlayerCharacteristics playerCharacteristics)
        {
            PlayerCharacteristics = playerCharacteristics;
            
            Health.CurrentHealthChanged += PlayerCharacteristics.SaveCurrentHealth;
            OnChangeSpeed += PlayerCharacteristics.UpdateCurrentSpeed;
        }

        public void AcceptImprovement(IWeaponVisitor weaponVisitor, CharacteristicType type, float value)
        {
            weaponVisitor.Visit(this, type, value);
        }

        public void ChangeFollowEnemyState(bool canFollow)
        {
            CanFollow = canFollow;
        }

        private void OnEnable()
        {
            Health.Die += Die;
        }

        private void OnDisable()
        {
            Health.Die -= Die;
        }

        private void OnDestroy()
        {
            Health.CurrentHealthChanged -= PlayerCharacteristics.SaveCurrentHealth;
            OnChangeSpeed -= PlayerCharacteristics.UpdateCurrentSpeed;
        }

        private void Die()
        {
            ResetModifiers();
            gameObject.SetActive(false);
        }
    }
}
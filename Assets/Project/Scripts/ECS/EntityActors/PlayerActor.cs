using Project.Scripts.Player.PlayerInputModule;
using Project.Scripts.Services;
using Project.Scripts.Weapon.CharacteristicsOfWeapon;
using Project.Scripts.Weapon.Improvements;
using UnityEngine;

namespace Project.Scripts.ECS.EntityActors
{
    public class PlayerActor : MonoBehaviour
    {
        [field: SerializeField] public Health.Health Health { get; private set; }
        [field: SerializeField] public Animator Animator { get; private set; }
        [field: SerializeField] public Rigidbody Rigidbody { get; private set; }
        [field: SerializeField] public PlayerInputController PlayerInputController { get; private set; }
        [field: SerializeField] public MiningToolActor MiningToolActor { get; private set; }
        
        public PlayerCharacteristics PlayerCharacteristics { get; private set; }

        public void Construct(IPlayerService playerService)
        {
            PlayerCharacteristics = new PlayerCharacteristics(playerService);
        }

        private void OnEnable()
        {
            Health.Die += Die;
        }

        private void OnDisable()
        {
            Health.Die -= Die;
        }

        private void Die()
        {
            gameObject.SetActive(false);
        }
        
        public void AcceptImprovement(IWeaponVisitor weaponVisitor, CharacteristicType type, float value)
        {
            weaponVisitor.Visit(this, type, value);
        }
    }
}
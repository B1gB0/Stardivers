using Project.Scripts.ParticleEffects.Effects;
using Project.Scripts.Player.PlayerInputModule;
using Project.Scripts.Services;
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
        
        private ParticleEffectsService _particleEffectsService;
        
        public PlayerCharacteristics PlayerCharacteristics { get; private set; }
        public bool CanFollow { get; private set; }

        public void Construct(ParticleEffectsService particleEffectsService, PlayerCharacteristics playerCharacteristics)
        {
            _particleEffectsService = particleEffectsService;
            
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
            Health.IsDamaged += OnPlayParticleEffect;
        }

        private void OnDisable()
        {
            Health.Die -= Die;
            Health.IsDamaged -= OnPlayParticleEffect;
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
        
        private void OnPlayParticleEffect()
        {
            _particleEffectsService.PlayEffect(ParticleEffectType.PlayerHit, Health.HitPoint.position);
        }
    }
}
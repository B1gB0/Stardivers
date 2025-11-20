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
        private const float RotationSpeed = 0.8f;
        
        [field: SerializeField] public Rigidbody Rigidbody { get; private set; }
        [field: SerializeField] public PlayerInputController PlayerInputController { get; private set; }
        [field: SerializeField] public MiningToolActor MiningToolActor { get; private set; }
        
        private ParticleEffectsService _particleEffectsService;
        private IPlayerService _playerService;
        
        public PlayerCharacteristics PlayerCharacteristics { get; private set; }
        public bool CanFollow { get; private set; }

        public void Construct(ParticleEffectsService particleEffectsService, IPlayerService playerService, 
            PlayerCharacteristics playerCharacteristics)
        {
            _particleEffectsService = particleEffectsService;
            _playerService = playerService;
            PlayerCharacteristics = playerCharacteristics;
            
            Health.CurrentHealthChanged += PlayerCharacteristics.SaveCurrentHealth;
            OnChangeSpeed += PlayerCharacteristics.UpdateCurrentSpeed;
        }
        
        private void OnEnable()
        {
            Health.Die += Die;
            Health.IsDamaged += OnPlayParticleEffect;
        }

        private void FixedUpdate()
        {
            if (!_playerService.CheckMoveSystem() && MiningToolActor.IsMining)
            {
                OnRotatePlayerToResource(MiningToolActor.TargetResource);
            }
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

        public void AcceptImprovement(IWeaponVisitor weaponVisitor, CharacteristicType type, float value)
        {
            weaponVisitor.Visit(this, type, value);
        }

        public void ChangeFollowEnemyState(bool canFollow)
        {
            CanFollow = canFollow;
        }

        private void OnRotatePlayerToResource(Transform target)
        {
            Vector3 direction = (target.transform.position - transform.position).normalized;
            
            direction.y = 0;

            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);

                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation,
                    Time.fixedDeltaTime * RotationSpeed);
            }
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
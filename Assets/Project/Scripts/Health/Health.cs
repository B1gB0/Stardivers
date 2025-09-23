using System;
using System.Collections;
using Build.Game.Scripts;
using Project.Scripts.UI.View;
using UnityEngine;

namespace Project.Scripts.Health
{
    public class Health : MonoBehaviour, IDamageable
    {
        private const float RecoveryRate = 10f;

        [SerializeField] private Color _healingColor;
        [SerializeField] private Color _standardColor;
        [SerializeField] private float _value;
        [SerializeField] private ParticleSystem _hitEffectPrefab;
        [SerializeField] private Transform _hitPoint;

        private ParticleSystem _hitEffect;
        private Coroutine _coroutine;
        private float _currentHealth;

        public event Action Die;

        public event Action<string, Transform, FloatingTextViewType, Color> IsSpawnedDamageText;
        public event Action<string, Transform, FloatingTextViewType, Color> IsSpawnedHealingText;

        public event Action IsDamaged; 

        public event Action<float, float, float> HealthChanged;
        
        public float MaxHealth { get; private set; }
        
        public float TargetHealth { get; private set; }

        public bool IsHitting { get; private set; }
        
        public bool IsHealing { get; private set; }

        private void Awake()
        {
            _hitEffect = Instantiate(_hitEffectPrefab);
            _hitEffect.Stop();
        }

        private void Start()
        {
            MaxHealth = _value;
            TargetHealth = _value;
            _currentHealth = _value;
            
            HealthChanged?.Invoke(_currentHealth, TargetHealth, MaxHealth);
        }

        public void TakeDamage(float damage)
        {
            IsSpawnedDamageText?.Invoke(damage.ToString(), transform, FloatingTextViewType.Damage, _standardColor);
            IsDamaged?.Invoke();

            _hitEffect.transform.position = _hitPoint.position;
            _hitEffect.Play();

            TargetHealth -= damage;

            OnChangeHealth();

            if (TargetHealth < 0f)
                TargetHealth = 0f;
        
            if(TargetHealth == 0)
                Die?.Invoke();
        }

        public void ImproveHealth(float newHealthValue)
        {
            var currentHealthPercentage = _currentHealth / MaxHealth;
            var maxHealth = MaxHealth + newHealthValue;
            
            MaxHealth = maxHealth;
            var currentHealth = MaxHealth * currentHealthPercentage;
            
            SetHealthValue(currentHealth);
        }

        public void AddHealth(float healthValue)
        {
            IsSpawnedHealingText?.Invoke(healthValue.ToString(), transform, FloatingTextViewType.Healing, _healingColor);
            TargetHealth += healthValue;
            
            OnChangeHealth();

            if (TargetHealth > MaxHealth)
                TargetHealth = MaxHealth;
        }

        public void SetHealthValue(float healthValue)
        {
            _value = healthValue;
            TargetHealth = _value;
            
            OnChangeHealth();
        }
        
        public void SetHit(bool isHitting)
        {
            IsHitting = isHitting;
        }

        private void OnChangeHealth()
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }

            _coroutine = StartCoroutine(ChangeHealth());
        }

        private IEnumerator ChangeHealth()
        {
            while (_currentHealth != TargetHealth)
            {
                _currentHealth = Mathf.MoveTowards(_currentHealth, TargetHealth, RecoveryRate * Time.deltaTime);
                HealthChanged?.Invoke(_currentHealth, MaxHealth, TargetHealth);

                yield return null;
            }
        }
    }
}

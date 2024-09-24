using System;
using System.Collections;
using UnityEngine;

namespace Build.Game.Scripts
{
    public class Health : MonoBehaviour, IDamageable
    {
        private const float RecoveryRate = 10f;
        
        [field: SerializeField] public float Value { get; private set; }
        
        [SerializeField] private ParticleSystem _hitEffect;
        [SerializeField] private Transform _hitPoint;

        private ParticleSystem _hitEffectRef;
        private Coroutine _coroutine;

        public event Action Die;

        public event Action<float, Transform> IsDamaged;
        
        public event Action<float, float, float> HealthChanged;

        private float _currentHealth;
        private float _maxHealth;
        private float _targetHealth;

        public bool IsHitting { get; private set; }

        private void Awake()
        {
            _hitEffectRef = Instantiate(_hitEffect);
            _hitEffectRef.Stop();
        }

        private void Start()
        {
            _maxHealth = Value;
            _targetHealth = Value;
            _currentHealth = Value;
            
            HealthChanged?.Invoke(_currentHealth, _targetHealth, _maxHealth);
        }

        public void TakeDamage(float damage)
        {
            IsDamaged?.Invoke(damage, transform);
            
            _hitEffectRef.transform.position = _hitPoint.position;
            _hitEffectRef.Play();
            
            Value -= damage;

            _targetHealth = Value;

            OnChangeHealth(_currentHealth, _targetHealth, _maxHealth);
            
            _currentHealth = Value;

            if (Value < 0f)
                Value = 0f;
        
            if(Value == 0)
                Die?.Invoke();
        }
        
        public void SetHit(bool isHitting)
        {
            IsHitting = isHitting;
        }

        private void OnChangeHealth(float currentHealth, float targetHealth, float maxHealth)
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }

            _coroutine = StartCoroutine(ChangeHealth(currentHealth, targetHealth, maxHealth));
        }

        private IEnumerator ChangeHealth(float currentHealth, float targetHealth, float maxHealth)
        {
            while (currentHealth != targetHealth)
            {
                currentHealth = Mathf.MoveTowards(currentHealth, targetHealth, RecoveryRate * Time.deltaTime);
                HealthChanged?.Invoke(currentHealth, maxHealth, targetHealth);

                yield return null;
            }
        }
    }
}

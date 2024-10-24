using System;
using System.Collections;
using UnityEngine;

namespace Build.Game.Scripts
{
    public class Health : MonoBehaviour, IDamageable
    {
        private const float RecoveryRate = 10f;
        
        [SerializeField] private float _value;
        [SerializeField] private ParticleSystem _hitEffect;
        [SerializeField] private Transform _hitPoint;

        private ParticleSystem _hitEffectRef;
        private Coroutine _coroutine;

        public event Action Die;

        public event Action<float, Transform> IsDamaged;
        
        public event Action<float, float, float> HealthChanged;

        private float _currentHealth;
        private float _maxHealth;
        
        public float TargetHealth { get; private set; }

        public bool IsHitting { get; private set; }

        private void Awake()
        {
            _hitEffectRef = Instantiate(_hitEffect);
            _hitEffectRef.Stop();
        }

        private void Start()
        {
            _maxHealth = _value;
            TargetHealth = _value;
            _currentHealth = _value;
            
            HealthChanged?.Invoke(_currentHealth, TargetHealth, _maxHealth);
        }

        public void TakeDamage(float damage)
        {
            IsDamaged?.Invoke(damage, transform);
            
            _hitEffectRef.transform.position = _hitPoint.position;
            _hitEffectRef.Play();

            TargetHealth -= damage;

            OnChangeHealth(_currentHealth, TargetHealth, _maxHealth);
            
            _currentHealth = TargetHealth;

            if (TargetHealth < 0f)
                TargetHealth = 0f;
        
            if(TargetHealth == 0)
                Die?.Invoke();
        }

        public void SetHealthValue()
        {
            TargetHealth = _value;
            
            OnChangeHealth(_currentHealth, TargetHealth, _maxHealth);

            _currentHealth = TargetHealth;
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

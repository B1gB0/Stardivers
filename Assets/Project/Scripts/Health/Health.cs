using System;
using System.Collections;
using System.Threading;
using Cysharp.Threading.Tasks;
using Project.Scripts.Game.Constant;
using Project.Scripts.Services;
using Project.Scripts.UI.View;
using Reflex.Attributes;
using UnityEngine;

namespace Project.Scripts.Health
{
    public class Health : MonoBehaviour, IDamageable
    {
        private const float RecoveryRate = 10f;
        
        [SerializeField] private float _value;
        [SerializeField] private Transform _hitPoint;
        
        private CancellationTokenSource _healthCts;
        private float _currentHealth;

        public event Action Die;
        public event Action<Health> DieHealth;

        public event Action<string, Transform, FloatingTextViewType, Color> IsSpawnedDamageText;
        public event Action<string, Transform, FloatingTextViewType, Color> IsSpawnedHealingText;

        public event Action IsDamaged; 

        public event Action<float, float, float> HealthChanged;
        public event Action<float> CurrentHealthChanged;
        
        public float MaxHealth { get; private set; }
        public float TargetHealth { get; private set; }

        public bool IsHitting { get; private set; }

        public Transform HitPoint => _hitPoint;

        private void Start()
        {
            HealthChanged?.Invoke(_currentHealth, MaxHealth, TargetHealth);
            CurrentHealthChanged?.Invoke(_currentHealth);
        }

        private void OnDestroy()
        {
            _healthCts?.Cancel();
        }

        public void TakeDamage(float damage)
        {
            IsSpawnedDamageText?.Invoke(damage.ToString(), transform, FloatingTextViewType.Damage,
                Colors.GetColor(ColorName.DefaultWhiteTextColor));

            IsDamaged?.Invoke();

            TargetHealth -= damage;

            OnChangeHealth();

            if (TargetHealth < 0f)
                TargetHealth = 0f;

            if (TargetHealth == 0)
            {
                Die?.Invoke();
                DieHealth?.Invoke(this);
            }
        }

        public void ImproveHealth(float newHealthValue)
        {
            var currentHealthPercentage = TargetHealth / MaxHealth;
            var maxHealth = MaxHealth + newHealthValue;
            
            MaxHealth = maxHealth;
            var currentHealth = MaxHealth * currentHealthPercentage;
            
            SetHealthValue(currentHealth);
        }

        public void LoadHealth(float maxHealth, float currentHealth)
        {
            MaxHealth = maxHealth;

            SetHealthValue(currentHealth);
        }

        public void AddHealth(float healthValue)
        {
            IsSpawnedHealingText?.Invoke(healthValue.ToString(), transform, FloatingTextViewType.Healing,
                Colors.GetColor(ColorName.HealingColor));
            
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
            _healthCts?.Cancel();
            _healthCts = new CancellationTokenSource();
            
            ChangeHealthAsync(_healthCts.Token).Forget();
        }

        private async UniTaskVoid ChangeHealthAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested && 
                   Math.Abs(_currentHealth - TargetHealth) > Mathf.Epsilon)
            {
                _currentHealth = Mathf.MoveTowards(
                    _currentHealth, 
                    TargetHealth, 
                    RecoveryRate * Time.unscaledDeltaTime
                );
                
                HealthChanged?.Invoke(_currentHealth, MaxHealth, TargetHealth);
                CurrentHealthChanged?.Invoke(_currentHealth);
                
                await UniTask.NextFrame(PlayerLoopTiming.Update, cancellationToken);
            }
        }
    }
}

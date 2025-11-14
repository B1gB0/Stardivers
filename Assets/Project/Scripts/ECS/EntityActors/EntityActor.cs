using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Project.Scripts.ECS.EntityActors
{
    public abstract class EntityActor : MonoBehaviour
    {
        private const float MinValue = 0f;
        
        private const int DurationFactor = 1000;
        
        [field: SerializeField] public Health.Health Health { get; private set; }
        [field: SerializeField] public Animator Animator { get; private set; }

        private float _currentModifier;
        private CancellationTokenSource _modifierCts;

        public event Action OnChangeSpeed;
        
        private void OnDestroy()
        {
            ResetModifiers();
        }

        public void ApplyTemporaryModifier(float modifier, float duration)
        {
            _modifierCts?.Cancel();
            _modifierCts = new CancellationTokenSource();
            
            TemporaryModifierTask(modifier, duration, _modifierCts.Token).Forget();
        }

        public float GetCurrentModifier() => _currentModifier;
        
        protected void ResetModifiers()
        {
            _modifierCts?.Cancel();
            _modifierCts?.Dispose();
            _modifierCts = null;
            
            _currentModifier = MinValue;
            OnChangeSpeed?.Invoke();
        }

        private async UniTaskVoid TemporaryModifierTask(float modifier, float duration,
            CancellationToken cancellationToken)
        {
            try
            {
                _currentModifier = modifier;
                OnChangeSpeed?.Invoke();

                await UniTask.Delay((int)(duration * DurationFactor), cancellationToken: cancellationToken);
                
                if (!cancellationToken.IsCancellationRequested)
                {
                    _currentModifier = MinValue;
                    OnChangeSpeed?.Invoke();
                }
            }
            finally
            {
                if (_modifierCts != null && _modifierCts.Token == cancellationToken)
                {
                    _modifierCts?.Dispose();
                    _modifierCts = null;
                }
            }
        }
    }
}
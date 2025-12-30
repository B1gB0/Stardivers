using System;
using Project.Scripts.Experience;
using Project.Scripts.ParticleEffects.Effects;
using Project.Scripts.Services;
using Project.Scripts.UI.View;
using UnityEngine;

namespace Project.Scripts.ECS.EntityActors
{
    public class AlienCocoon : ResourceActor, IAcceptable
    {
        private IFloatingTextService _textService;
        private ICurrencyService _currencyService;

        public event Action<AlienCocoon> OnDied;

        [field: SerializeField] public Color Color { get; private set; }

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

        public void AcceptScore(IScoreActorVisitor visitor)
        {
            visitor.Visit(this);
        }

        public void GetServices(ICurrencyService currencyService, IFloatingTextService textService)
        {
            _currencyService = currencyService;
            _textService = textService;
        }

        protected override void OnPlayParticleEffect()
        {
            ParticleEffectsService.PlayEffect(ParticleEffectType.EnemyHit, Health.HitPoint.position);
        }

        private void Die()
        {
            _textService.OnChangedFloatingText(
                "+" + Data.CrystalValue,
                transform,
                FloatingTextViewType.AlienCocoon, Color);
            
            _currencyService.AddAlienCocoon((int)Data.CrystalValue);
            ExperiencePoints.OnKill(this);
            gameObject.SetActive(false);

            OnDied?.Invoke(this);
        }
    }
}
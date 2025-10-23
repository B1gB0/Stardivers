using Project.Scripts.Experience;
using Project.Scripts.Services;
using Project.Scripts.UI.View;
using UnityEngine;

namespace Project.Scripts.ECS.EntityActors
{
    public class AlienCocoon : ResourceActor, IAcceptable
    {
        [field: SerializeField] public Color Color { get; private set; }
        
        private IFloatingTextService _textService;
        private ICurrencyService _currencyService;
        
        private void OnEnable()
        {
            Health.Die += Die;
        }

        private void OnDisable()
        {
            Health.Die -= Die;
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

        private void Die()
        {
            _textService.OnChangedFloatingText("+" + Data.CrystalValue, transform,
                FloatingTextViewType.AlienCocoon, Color);
            _currencyService.AddAlienCocoon((int)Data.CrystalValue);
            ExperiencePoints.OnKill(this);
            gameObject.SetActive(false);
        }
    }
}
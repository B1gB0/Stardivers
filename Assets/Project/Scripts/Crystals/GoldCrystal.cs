using Project.Scripts.Services;
using Project.Scripts.UI.View;
using UnityEngine;

namespace Project.Scripts.Crystals
{
    [RequireComponent(typeof(Rigidbody))]
    public class GoldCrystal : Crystal
    {
        [field: SerializeField] public Color Color { get; private set; }
        
        private ICurrencyService _currencyService;
        
        public int GoldValue { get; private set; }

        public void Destroy()
        {
            _currencyService.AddGold(GoldValue);
            TextService.OnChangedFloatingText("+" + GoldValue, transform, FloatingTextViewType.Gold, Color);
            Destroy(gameObject);
        }

        public void GetCurrencyService(ICurrencyService currencyService, int goldValue)
        {
            _currencyService = currencyService;
            GoldValue = goldValue;
        }
    }
}
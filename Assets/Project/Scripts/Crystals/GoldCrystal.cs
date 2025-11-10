using Project.Scripts.Game.Constant;
using Project.Scripts.Services;
using Project.Scripts.UI.View;
using UnityEngine;

namespace Project.Scripts.Crystals
{
    [RequireComponent(typeof(Rigidbody))]
    public class GoldCrystal : Crystal
    {
        private ICurrencyService _currencyService;

        private int _goldValue;

        public void Destroy()
        {
            _currencyService.AddGold(_goldValue);
            
            TextService.OnChangedFloatingText("+" + _goldValue, transform, FloatingTextViewType.Gold, 
                Colors.GetColor(ColorName.GoldColor));
            
            Destroy(gameObject);
        }

        public void GetCurrencyService(ICurrencyService currencyService, int goldValue)
        {
            _currencyService = currencyService;
            _goldValue = goldValue;
        }
    }
}
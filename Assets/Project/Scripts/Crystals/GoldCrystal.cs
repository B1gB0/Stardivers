using Project.Scripts.Services;
using Project.Scripts.UI.View;
using UnityEngine;

namespace Project.Scripts.Crystals
{
    [RequireComponent(typeof(Rigidbody))]
    public class GoldCrystal : Crystal
    {
        [field: SerializeField] public int GoldValue { get; private set; }
        [field: SerializeField] public Color Color { get; private set; }
        
        private IGoldService _goldService;

        public void Destroy()
        {
            _goldService.AddGold(GoldValue);
            TextService.OnChangedFloatingText("+" + GoldValue, transform, FloatingTextViewType.Gold, Color);
            Destroy(gameObject);
        }

        public void GetGoldService(IGoldService goldService)
        {
            _goldService = goldService;
        }
    }
}
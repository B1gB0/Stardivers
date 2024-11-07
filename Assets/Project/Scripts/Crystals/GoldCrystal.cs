using Project.Scripts.UI.View;
using UnityEngine;

namespace Project.Scripts.Crystals
{
    [RequireComponent(typeof(Rigidbody))]
    public class GoldCrystal : Crystal
    {
        [field: SerializeField] public int GoldValue { get; private set; }

        private GoldView _goldView;

        public void GetGoldView(GoldView goldView)
        {
            _goldView = goldView;
        }

        public void Destroy()
        {
            _goldView.SetValue(GoldValue);
            TextService.OnChangedFloatingText("+" + GoldValue, transform, ColorText);
            Destroy(gameObject);
        }
    }
}
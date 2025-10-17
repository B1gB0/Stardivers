using UnityEngine;

namespace Project.Scripts.Crystals
{
    [RequireComponent(typeof(Rigidbody))]
    public class RedCrystal : Crystal
    {
        [field: SerializeField] public Color Color { get; private set; }
        
        public float HealthValue { get; private set; }
        
        public void Destroy()
        {
            // TextService.OnChangedFloatingText("+" + HealthValue, transform, FloatingTextViewType.RedCrystal, Color);
            Destroy(gameObject);
        }

        public void GetHealthValue(float healthValue)
        {
            HealthValue = healthValue;
        }
    }
}
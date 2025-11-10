using UnityEngine;

namespace Project.Scripts.Crystals
{
    [RequireComponent(typeof(Rigidbody))]
    public class RedCrystal : Crystal
    {
        public float HealthValue { get; private set; }
        
        public void Destroy()
        {
            Destroy(gameObject);
        }

        public void GetHealthValue(float healthValue)
        {
            HealthValue = healthValue;
        }
    }
}
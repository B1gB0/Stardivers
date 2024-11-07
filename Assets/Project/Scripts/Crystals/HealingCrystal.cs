using System;
using Project.Scripts.Services;
using UnityEngine;

namespace Project.Scripts.Crystals
{
    [RequireComponent(typeof(Rigidbody))]
    public class HealingCrystal : Crystal
    {
        [field: SerializeField] public float HealthValue { get; private set; }
        
        public void Destroy()
        {
            TextService.OnChangedFloatingText("+" + HealthValue, transform, ColorText);
            Destroy(gameObject);
        }
    }
}
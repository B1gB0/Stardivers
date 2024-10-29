using System;
using Build.Game.Scripts.ECS.EntityActors;
using UnityEngine;

namespace Project.Scripts.MiningResources
{
    public class HealingCrystal : MonoBehaviour
    {
        [SerializeField] private float _healthValue;
        
        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out PlayerActor player)) return;
            
            if (!player.Health.IsHealing) return;
            
            player.Health.AddHealth(_healthValue);
            Destroy(gameObject);
        }
    }
}
using System;
using UnityEngine;

namespace Project.Scripts.Cards.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Cards/Improvement Card")]
    public class ImprovementCard : Card
    {
        [field: SerializeField] public string WeaponType { get; private set; }
        
        [field: SerializeField] public string CharacteristicsType { get; private set; }
        
        [field: SerializeField] public float Value { get; private set; }
    }
}
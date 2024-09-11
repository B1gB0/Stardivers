using System;
using Project.Game.Scripts;
using UnityEngine;

namespace Project.Scripts.Cards.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Cards/Improvement Card")]
    public class ImprovementCard : Card
    {
        [field: SerializeField] public Weapons WeaponType { get; private set; }
        
        [field: SerializeField] public CharacteristicsTypes CharacteristicsType { get; private set; }
        
        [field: SerializeField] public float Value { get; private set; }
    }
}
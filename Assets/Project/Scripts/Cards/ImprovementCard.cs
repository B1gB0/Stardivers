﻿using System;
using Project.Game.Scripts;
using Project.Scripts.Weapon.Characteristics;
using Project.Scripts.Weapon.Player;
using UnityEngine;

namespace Project.Scripts.Cards.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Cards/Improvement Card")]
    public class ImprovementCard : Card
    {
        [field: SerializeField] public WeaponType WeaponType { get; private set; }
        [field: SerializeField] public CharacteristicType CharacteristicType { get; private set; }
        [field: SerializeField] public float Value { get; private set; }
    }
}
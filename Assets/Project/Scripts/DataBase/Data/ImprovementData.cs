using System;
using Project.Scripts.Cards;
using Project.Scripts.Weapon.Characteristics;
using Project.Scripts.Weapon.Player;
using UnityEngine;

namespace Project.Scripts.DataBase.Data
{
    [Serializable]
    public class ImprovementData
    {
        [SerializeField] private string _id;
        [SerializeField] private WeaponType _weaponType;
        [SerializeField] private CharacteristicType _characteristicType;
        [SerializeField] private LevelCard _levelCard;
        [SerializeField] private float _value;
        
        public string Id => _id;
        public WeaponType WeaponType => _weaponType;
        public CharacteristicType CharacteristicType => _characteristicType;
        public LevelCard LevelCard => _levelCard;
        public float Value => _value;
    }
}
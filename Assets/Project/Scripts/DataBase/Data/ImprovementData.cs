using System;
using Project.Scripts.Weapon.CharacteristicsOfWeapon;
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
        [SerializeField] private string _levelCardRu;
        [SerializeField] private string _levelCardEn;
        [SerializeField] private string _levelCardTr;
        [SerializeField] private float _value;
        
        public string Id => _id;
        public WeaponType WeaponType => _weaponType;
        public CharacteristicType CharacteristicType => _characteristicType;
        public string LevelCardRu => _levelCardRu;
        public string LevelCardEn => _levelCardEn;
        public string LevelCardTr => _levelCardTr;
        public float Value => _value;
    }
}
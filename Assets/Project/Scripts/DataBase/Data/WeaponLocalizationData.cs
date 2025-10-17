using System;
using Project.Scripts.Weapon.Player;
using UnityEngine;

namespace Project.Scripts.DataBase.Data
{
    [Serializable]
    public class WeaponLocalizationData
    {
        [SerializeField] private string _id;
        [SerializeField] private WeaponType _type;
        [SerializeField] private string _nameRu;
        [SerializeField] private string _nameEn;
        [SerializeField] private string _nameTr;
        [SerializeField] private string _descriptionRu;
        [SerializeField] private string _descriptionEn;
        [SerializeField] private string _descriptionTr;
        
        public string Id => _id;
        public WeaponType Type => _type;
        public string NameRu => _nameRu;
        public string NameEn => _nameEn;
        public string NameTr => _nameTr;
        public string DescriptionRu => _descriptionRu;
        public string DescriptionEn => _descriptionEn;
        public string DescriptionTr => _descriptionTr;
    }
}
using System;
using UnityEngine;

namespace Project.Scripts.DataBase.Data
{
    [Serializable]
    public class OperationLocalizationData
    {
        [SerializeField] private string _id;
        [SerializeField] private string _nameRu;
        [SerializeField] private string _nameEn;
        [SerializeField] private string _nameTr;
        [SerializeField] private string _descriptionRu;
        [SerializeField] private string _descriptionEn;
        [SerializeField] private string _descriptionTr;
        [SerializeField] private int _price;
        
        public string Id => _id;
        public string NameRu => _nameRu;
        public string NameEn => _nameEn;
        public string NameTr => _nameTr;
        public string DescriptionRu => _descriptionRu;
        public string DescriptionEn => _descriptionEn;
        public string DescriptionTr => _descriptionTr;
        public int Price => _price;
    }
}
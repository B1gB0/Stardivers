using System;
using Project.Scripts.UI;
using UnityEngine;

namespace Project.Scripts.DataBase.Data
{
    [Serializable]
    public class UILocalizationData
    {
        [SerializeField] private string _id;
        [SerializeField] private UITextType _uiTextType;
        [SerializeField] private string _nameRu;
        [SerializeField] private string _nameEn;
        [SerializeField] private string _nameTr;

        public string Id => _id;
        public UITextType UITextType => _uiTextType;
        public string NameRu => _nameRu;
        public string NameEn => _nameEn;
        public string NameTr => _nameTr;
    }
}
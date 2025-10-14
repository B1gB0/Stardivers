using System;
using Project.Scripts.Levels;
using UnityEngine;

namespace Project.Scripts.DataBase.Data
{
    [Serializable]
    public class LevelTextData
    {
        [SerializeField] private string _id;
        [SerializeField] private LevelTextsType _type;
        [SerializeField] private string _sceneName;
        [SerializeField] private string _textRu;
        [SerializeField] private string _textEn;
        [SerializeField] private string _textTr;
        
        public string Id => _id;
        public LevelTextsType Type => _type;
        public string SceneName => _sceneName;
        public string TextRu => _textRu;
        public string TextEn => _textEn;
        public string TextTr => _textTr;
    }
}
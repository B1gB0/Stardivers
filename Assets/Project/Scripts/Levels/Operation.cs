using System.Collections.Generic;
using Project.Scripts.DataBase.Data;
using Project.Scripts.ECS.Data;
using UnityEngine;

namespace Project.Scripts.Levels
{
    [CreateAssetMenu(menuName = "Operations/New Operation")]
    public class Operation : ScriptableObject
    {
        [field: SerializeField] public string Id { get; private set; }
        [field: SerializeField] public List<LevelInitData> Maps { get; private set; } = new();
        [field: SerializeField] public string NameRu { get; private set; }
        [field: SerializeField] public string NameEn { get; private set; }
        [field: SerializeField] public string NameTr { get; private set; }
        [field: SerializeField] public string DescriptionRu { get; private set; }
        [field: SerializeField] public string DescriptionEn { get; private set; }
        [field: SerializeField] public string DescriptionTr { get; private set; }
        [field: SerializeField] public int Price { get; private set; }
        [field: SerializeField] public Sprite Image { get; private set; }


        public void SetData(OperationLocalizationData data)
        {
            NameRu = data.NameRu;
            NameEn = data.NameEn;
            NameTr = data.NameTr;
            
            DescriptionRu = data.DescriptionRu;
            DescriptionEn = data.DescriptionEn;
            DescriptionTr = data.DescriptionTr;
            
            Price = data.Price;
        }
    }
}
using Project.Scripts.ECS.EntityActors;
using UnityEngine;

namespace Project.Scripts.ECS.Data
{
    [CreateAssetMenu(menuName = "InitData/GoldCoreData")]
    public class GoldCoreInitData : InitData
    {
        [field: SerializeField] public GoldCore GoldCorePrefab { get; private set; }
    }
}
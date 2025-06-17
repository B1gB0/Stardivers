using Project.Scripts.ECS.EntityActors;
using UnityEngine;

namespace Project.Scripts.ECS.Data
{
    [CreateAssetMenu(menuName = "InitData/HealingCoreData")]
    public class HealingCoreInitData : InitData
    {
        [field: SerializeField] public HealingCore HealingCorePrefab { get; private set; }
    }
}
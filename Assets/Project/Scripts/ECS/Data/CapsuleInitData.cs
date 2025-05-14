using Project.Scripts.ECS.EntityActors;
using UnityEngine;

namespace Project.Scripts.ECS.Data
{
    [CreateAssetMenu(menuName = "InitData/CapsuleData")]
    public class CapsuleInitData : InitData
    {
        [field: SerializeField] public CapsuleActor Prefab { get; private set; }
        [field: SerializeField] public float DefaultMoveSpeed { get; private set; } = 5f;
    }
}

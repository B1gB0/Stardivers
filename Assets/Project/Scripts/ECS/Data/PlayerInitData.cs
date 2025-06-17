using Project.Scripts.ECS.EntityActors;
using UnityEngine;

namespace Project.Scripts.ECS.Data
{
    [CreateAssetMenu(menuName = "InitData/PlayerData")]
    public class PlayerInitData : InitData
    {
        [field: SerializeField] public PlayerActor Prefab { get; private set; }
        [field: SerializeField] public Health.Health Health { get; private set; }
        [field: SerializeField] public float DefaultMoveSpeed { get; private set; } = 3f;
        [field: SerializeField] public float DefaultRotationSpeed { get; private set; } = 3f;
    }
}
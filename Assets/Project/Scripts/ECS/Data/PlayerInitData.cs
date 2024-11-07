using Build.Game.Scripts.ECS.EntityActors;
using Project.Scripts.ECS.Data;
using Project.Scripts.ECS.EntityActors;
using Project.Scripts.Health;
using UnityEngine;

namespace Build.Game.Scripts.ECS.Data
{
    [CreateAssetMenu(menuName = "InitData/PlayerData")]
    public class PlayerInitData : InitData
    {
        [field: SerializeField] public PlayerActor Prefab { get; private set; }
        
        [field: SerializeField] public Health Health { get; private set; }

        [field: SerializeField] public float DefaultMoveSpeed { get; private set; } = 3f;
        
        [field: SerializeField] public float DefaultRotationSpeed { get; private set; } = 3f;
    }
}
using Project.Scripts.ECS.EntityActors;
using UnityEngine;

namespace Project.Scripts.ECS.Data
{
    [CreateAssetMenu(menuName = "InitData/PlayerData")]
    public class PlayerInitData : InitData
    {
        [field: SerializeField] public PlayerActor Prefab { get; private set; }
    }
}
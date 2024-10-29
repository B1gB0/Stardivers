using Build.Game.Scripts.ECS.EntityActors;
using Project.Scripts.ECS.Data;
using UnityEngine;

namespace Build.Game.Scripts.ECS.Data
{
    [CreateAssetMenu(menuName = "InitData/StoneData")]
    public class StoneInitData : InitData
    {
        [field: SerializeField] public StoneActor StoneActorPrefab { get; private set; }
    }
}
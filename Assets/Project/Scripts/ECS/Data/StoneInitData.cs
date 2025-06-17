using Project.Scripts.ECS.EntityActors;
using UnityEngine;

namespace Project.Scripts.ECS.Data
{
    [CreateAssetMenu(menuName = "InitData/StoneData")]
    public class StoneInitData : InitData
    {
        [field: SerializeField] public StoneActor StoneActorPrefab { get; private set; }
    }
}
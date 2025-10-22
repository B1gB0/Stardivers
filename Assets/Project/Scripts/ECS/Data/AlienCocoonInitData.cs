using Project.Scripts.ECS.EntityActors;
using UnityEngine;

namespace Project.Scripts.ECS.Data
{
    [CreateAssetMenu(menuName = "InitData/AlienCocoonInitData")]
    public class AlienCocoonInitData : InitData
    {
        [field: SerializeField] public AlienCocoon AlienCocoonPrefab { get; private set; }
    }
}
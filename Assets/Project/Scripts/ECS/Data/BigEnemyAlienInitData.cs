using Build.Game.Scripts.ECS.EntityActors;
using Project.Scripts.ECS.EntityActors;
using UnityEngine;

namespace Project.Scripts.ECS.Data
{
    [CreateAssetMenu(menuName = "InitData/BigAlienEnemyData")]
    public class BigEnemyAlienInitData : InitData
    {
        [field: SerializeField] public BigAlienEnemyAlien BigAlienEnemyPrefab { get; private set; }

        [field: SerializeField] public float DefaultDamage { get; private set; } = 15f;
    }
}
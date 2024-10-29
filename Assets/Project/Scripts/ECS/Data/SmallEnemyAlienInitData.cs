using Build.Game.Scripts.ECS.EntityActors;
using Project.Scripts.ECS.Data;
using Project.Scripts.ECS.EntityActors;
using Project.Scripts.UI;
using TMPro;
using UnityEngine;

namespace Build.Game.Scripts.ECS.Data.SO
{
    [CreateAssetMenu(menuName = "InitData/SmallAlienEnemyData")]
    public class SmallEnemyAlienInitData : InitData
    {
        [field: SerializeField] public SmallAlienEnemy SmallAlienEnemyPrefab { get; private set; }

        [field: SerializeField] public float DefaultMoveSpeed { get; private set; } = 3f;
        
        [field: SerializeField] public float DefaultRotationSpeed { get; private set; } = 3f;
        
        [field: SerializeField] public float DefaultDamage { get; private set; } = 1f;
    }
}
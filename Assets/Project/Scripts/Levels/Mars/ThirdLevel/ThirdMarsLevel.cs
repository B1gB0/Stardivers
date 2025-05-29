using Project.Scripts.Levels.Triggers;
using UnityEngine;

namespace Project.Scripts.Levels.Mars.ThirdLevel
{
    public class ThirdMarsLevel : Level
    {
        [SerializeField] private EnemySpawnTrigger _enemySpawnTrigger;
        [SerializeField] private EntranceTrigger _entranceLastLvlTrigger;
    }
}
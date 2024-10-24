using System.Collections.Generic;
using Project.Scripts.ECS.Data;
using Project.Scripts.Operations;
using UnityEngine;

[CreateAssetMenu(menuName = "InitData/LevelData")]
public class LevelInitData : InitData
{
    [field: SerializeField] public Level LevelPrefab { get; private set; }

    public List<Vector3> SmallEnemyAlienSpawnPoints;
    
    public List<Vector3> BigEnemyAlienSpawnPoints;

    public List<Vector3> StoneSpawnPoints;

    public Vector3 PlayerSpawnPoint;
}

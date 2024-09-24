using System.Collections.Generic;
using Project.Scripts.ECS.Data;
using UnityEngine;

[CreateAssetMenu(menuName = "InitData/LevelData")]
public class MapInitData : InitData
{
    [field: SerializeField] public string Label { get; private set; }
    
    [field: SerializeField] public Sprite Image { get; private set; }
    
    [field: SerializeField] public GameObject MapPrefab { get; private set; }

    public List<Vector3> EnemySpawnPoints;

    public List<Vector3> StoneSpawnPoints;

    public Vector3 PlayerSpawnPoint;
}

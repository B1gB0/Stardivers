using System.Collections;
using System.Collections.Generic;
using Build.Game.Scripts.ECS.EntityActors;
using Project.Scripts.ECS.Data;
using UnityEngine;

[CreateAssetMenu(menuName = "InitData/LevelData")]
public class LevelInitData : InitData
{
    //[field: SerializeField] public int Index { get; private set; }
    
    //[field: SerializeField] public string Label { get; private set; }
    
    //[field: SerializeField] public Sprite Image { get; private set; }
    
    //[field: SerializeField] public Object Plane { get; private set; }

    public List<Vector3> EnemySpawnPoints;

    public List<Vector3> ResourcesSpawnPoints;

    public Vector3 PlayerSpawnPoint;
}

using System.Collections;
using System.Collections.Generic;
using Build.Game.Scripts.ECS.EntityActors;
using Project.Scripts.ECS.Data;
using UnityEngine;

[CreateAssetMenu(menuName = "InitData/CapsuleData")]
public class CapsuleInitData : InitData
{
    [field: SerializeField] public CapsuleActor Prefab { get; private set; }

    [field: SerializeField] public float DefaultMoveSpeed { get; private set; } = 5f;
}

using System;
using System.Collections;
using System.Collections.Generic;
using Build.Game.Scripts.ECS.EntityActors;
using UnityEngine;

public class LevelDataForConfig : MonoBehaviour
{
    [SerializeField] private LevelInitData _levelInitData;

    [ContextMenu("Save Data")]
    public void SaveDataToConfigLevel()
    {
        GameObject[] enemySpawnPoints = GameObject.FindGameObjectsWithTag("EnemySpawnPoint");
        GameObject[] stoneSpawnPoints = GameObject.FindGameObjectsWithTag("StoneSpawnPoint");
        GameObject playerSpawnPoint = GameObject.FindGameObjectWithTag("PlayerSpawnPoint");;

        foreach (var enemy in enemySpawnPoints)
        {
            _levelInitData.EnemySpawnPoints.Add(enemy.transform.position);
        }

        foreach (var resource in stoneSpawnPoints)
        {
            _levelInitData.ResourcesSpawnPoints.Add(resource.transform.position);
        }

        _levelInitData.PlayerSpawnPoint = playerSpawnPoint.transform.position;
    }
}

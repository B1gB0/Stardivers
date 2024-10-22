using UnityEngine;

public class LevelDataForConfig : MonoBehaviour
{
    [SerializeField] private LevelInitData levelInitData;

    [ContextMenu("Save Data")]
    public void SaveDataToConfigLevel()
    {
        GameObject[] enemySpawnPoints = GameObject.FindGameObjectsWithTag("EnemySpawnPoint");
        GameObject[] stoneSpawnPoints = GameObject.FindGameObjectsWithTag("StoneSpawnPoint");
        GameObject playerSpawnPoint = GameObject.FindGameObjectWithTag("PlayerSpawnPoint");
        
        levelInitData.PlayerSpawnPoint = playerSpawnPoint.transform.position;
        
        foreach (var enemy in enemySpawnPoints)
        {
            levelInitData.EnemySpawnPoints.Add(enemy.transform.position);
        }

        foreach (var stone in stoneSpawnPoints)
        {
            levelInitData.StoneSpawnPoints.Add(stone.transform.position);
        }
    }
}

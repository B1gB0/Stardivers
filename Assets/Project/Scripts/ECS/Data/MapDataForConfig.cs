using UnityEngine;

public class MapDataForConfig : MonoBehaviour
{
    [SerializeField] private MapInitData _mapInitData;

    [ContextMenu("Save Data")]
    public void SaveDataToConfigLevel()
    {
        GameObject[] enemySpawnPoints = GameObject.FindGameObjectsWithTag("EnemySpawnPoint");
        GameObject[] stoneSpawnPoints = GameObject.FindGameObjectsWithTag("StoneSpawnPoint");
        GameObject playerSpawnPoint = GameObject.FindGameObjectWithTag("PlayerSpawnPoint");
        
        _mapInitData.PlayerSpawnPoint = playerSpawnPoint.transform.position;
        
        foreach (var enemy in enemySpawnPoints)
        {
            _mapInitData.EnemySpawnPoints.Add(enemy.transform.position);
        }

        foreach (var stone in stoneSpawnPoints)
        {
            _mapInitData.StoneSpawnPoints.Add(stone.transform.position);
        }
    }
}

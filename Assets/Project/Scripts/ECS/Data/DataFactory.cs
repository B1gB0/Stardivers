using Build.Game.Scripts.ECS.Data;
using Build.Game.Scripts.ECS.Data.SO;
using Project.Scripts.ECS.Data;
using Project.Scripts.Operations;
using UnityEngine;

public class DataFactory : MonoBehaviour
{
    private const string Mars = nameof(Mars);
    private const string MysteryPlanet = nameof(MysteryPlanet);
    
    private readonly string _playerData = "SO/PlayerData";
    private readonly string _enemyData = "SO/EnemyData";
    private readonly string _stoneData = "SO/StoneData";
    private readonly string _capsuleData = "SO/CapsuleData";
    private readonly string _playerProgression = "SO/PlayerProgression";

    [SerializeField] private Operation _operationMars;
    [SerializeField] private Operation _operationMysteryPlanet;

    public PlayerInitData CreatePlayerData()
    {
        return Resources.Load<PlayerInitData>(_playerData);
    }
    
    public MapInitData CreateMapData(string name, int index)
    {
        return name switch
        {
            Mars => Instantiate(_operationMars.Maps[index]),
            MysteryPlanet => Instantiate(_operationMysteryPlanet.Maps[index]),
            _ => null
        };
    }
    
    public EnemyInitData CreateEnemyData()
    {
        return Resources.Load<EnemyInitData>(_enemyData);
    }
    
    public StoneInitData CreateStoneData()
    {
        return Resources.Load<StoneInitData>(_stoneData);
    }

    public CapsuleInitData CreateCapsuleData()
    {
        return Resources.Load<CapsuleInitData>(_capsuleData);
    }

    public PlayerProgressionInitData CreatePlayerProgression()
    {
        return Resources.Load<PlayerProgressionInitData>(_playerProgression);
    }
}

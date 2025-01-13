using Build.Game.Scripts.ECS.Data;
using Build.Game.Scripts.ECS.Data.SO;
using Project.Scripts.ECS.Data;
using Project.Scripts.Levels;
using UnityEngine;

public class DataFactory : MonoBehaviour
{
    private readonly string _playerData = "SO/PlayerData";
    private readonly string _smallEnemyAlienData = "SO/SmallEnemyAlienData";
    private readonly string _bigEnemyAlienData = "SO/BigEnemyAlienData";
    private readonly string _gunnerEnemyAlienData = "SO/GunnerEnemyAlienData";
    private readonly string _stoneData = "SO/StoneData";
    private readonly string _capsuleData = "SO/CapsuleData";
    private readonly string _playerProgression = "SO/PlayerProgression";
    private readonly string _healingCoreData = "SO/HealingCoreData";
    private readonly string _goldCoreData = "SO/GoldCoreData";

    public PlayerInitData CreatePlayerData()
    {
        return Resources.Load<PlayerInitData>(_playerData);
    }
    
    public LevelInitData CreateLevelData(Operation operation, int numberLevel)
    {
        LevelInitData levelInitData = Instantiate(operation.Maps[numberLevel]);
        return levelInitData;
    }
    
    public SmallEnemyAlienInitData CreateSmallEnemyAlienData()
    {
        return Resources.Load<SmallEnemyAlienInitData>(_smallEnemyAlienData);
    }
    
    public BigEnemyAlienInitData CreateBigEnemyAlienData()
    {
        return Resources.Load<BigEnemyAlienInitData>(_bigEnemyAlienData);
    }

    public GunnerAlienEnemyInitData CreateGunnerAlienEnemyData()
    {
        return Resources.Load<GunnerAlienEnemyInitData>(_gunnerEnemyAlienData);
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

    public HealingCoreInitData CreateHealingCoreData()
    {
        return Resources.Load<HealingCoreInitData>(_healingCoreData);
    }
    
    public GoldCoreInitData CreateGoldCoreData()
    {
        return Resources.Load<GoldCoreInitData>(_goldCoreData);
    }
}

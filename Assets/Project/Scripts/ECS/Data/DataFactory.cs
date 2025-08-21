using Cysharp.Threading.Tasks;
using Project.Scripts.ECS.Data;
using Project.Scripts.Levels;
using Project.Scripts.Services;
using Reflex.Attributes;
using UnityEngine;

public class DataFactory : MonoBehaviour
{
    private readonly string _playerData = "PlayerData";
    private readonly string _smallEnemyAlienData = "SmallEnemyAlienData";
    private readonly string _bigEnemyAlienData = "BigEnemyAlienData";
    private readonly string _gunnerEnemyAlienData = "GunnerEnemyAlienData";
    private readonly string _stoneData = "StoneData";
    private readonly string _capsuleData = "CapsuleData";
    private readonly string _playerProgression = "PlayerProgression";
    private readonly string _healingCoreData = "HealingCoreData";
    private readonly string _goldCoreData = "GoldCoreData";
    
    private IResourceService _resourceService;
    
    [Inject]
    private void Construct(IResourceService resourceService)
    {
        _resourceService = resourceService;
    }
    
    public LevelInitData CreateLevelData(Operation operation, int numberLevel)
    {
        LevelInitData levelInitData = Instantiate(operation.Maps[numberLevel]);
        return levelInitData;
    }

    public async UniTask<PlayerInitData> CreatePlayerData()
    {
        return await _resourceService.Load<PlayerInitData>(_playerData);
    }

    public async UniTask<SmallAlienEnemyInitData> CreateSmallEnemyAlienData()
    {
        return await _resourceService.Load<SmallAlienEnemyInitData>(_smallEnemyAlienData);
    }
    
    public async UniTask<BigAlienEnemyInitData> CreateBigEnemyAlienData()
    {
        return await _resourceService.Load<BigAlienEnemyInitData>(_bigEnemyAlienData);
    }

    public async UniTask<GunnerAlienEnemyInitData> CreateGunnerAlienEnemyData()
    {
        return await _resourceService.Load<GunnerAlienEnemyInitData>(_gunnerEnemyAlienData);
    }
    
    public async UniTask<StoneInitData> CreateStoneData()
    {
        return await _resourceService.Load<StoneInitData>(_stoneData);
    }

    public async UniTask<CapsuleInitData> CreateCapsuleData()
    {
        return await _resourceService.Load<CapsuleInitData>(_capsuleData);
    }

    public async UniTask<PlayerProgressionInitData> CreatePlayerProgression()
    {
        return await _resourceService.Load<PlayerProgressionInitData>(_playerProgression);
    }

    public async UniTask<HealingCoreInitData> CreateHealingCoreData()
    {
        return await _resourceService.Load<HealingCoreInitData>(_healingCoreData);
    }
    
    public async UniTask<GoldCoreInitData> CreateGoldCoreData()
    {
        return await _resourceService.Load<GoldCoreInitData>(_goldCoreData);
    }
}

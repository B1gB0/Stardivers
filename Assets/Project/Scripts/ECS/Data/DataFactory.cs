using Cysharp.Threading.Tasks;
using Project.Scripts.Levels;
using Project.Scripts.Services;
using Reflex.Attributes;
using UnityEngine;

namespace Project.Scripts.ECS.Data
{
    public class DataFactory : MonoBehaviour
    {
        private const string PlayerData = "PlayerData";
        private const string SmallEnemyAlienData = "SmallEnemyAlienData";
        private const string BigEnemyAlienData = "BigEnemyAlienData";
        private const string GunnerEnemyAlienData = "GunnerEnemyAlienData";
        private const string StoneData = "StoneData";
        private const string CapsuleData = "CapsuleData";
        private const string HealingCoreData = "HealingCoreData";
        private const string GoldCoreData = "GoldCoreData";
        private const string AlienCocoonData = "AlienCocoonData";
        private const string AlienTurretEnemyData = "AlienTurretEnemyData";
        private const string IceCrystalData = "IceCrystalData";

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
            return await _resourceService.Load<PlayerInitData>(PlayerData);
        }

        public async UniTask<SmallAlienEnemyInitData> CreateSmallEnemyAlienData()
        {
            return await _resourceService.Load<SmallAlienEnemyInitData>(SmallEnemyAlienData);
        }

        public async UniTask<BigAlienEnemyInitData> CreateBigEnemyAlienData()
        {
            return await _resourceService.Load<BigAlienEnemyInitData>(BigEnemyAlienData);
        }

        public async UniTask<GunnerAlienEnemyInitData> CreateGunnerAlienEnemyData()
        {
            return await _resourceService.Load<GunnerAlienEnemyInitData>(GunnerEnemyAlienData);
        }

        public async UniTask<AlienTurretEnemyInitData> CreateAlienTurretEnemyData()
        {
            return await _resourceService.Load<AlienTurretEnemyInitData>(AlienTurretEnemyData);
        }

        public async UniTask<StoneInitData> CreateStoneData()
        {
            return await _resourceService.Load<StoneInitData>(StoneData);
        }

        public async UniTask<CapsuleInitData> CreateCapsuleData()
        {
            return await _resourceService.Load<CapsuleInitData>(CapsuleData);
        }

        public async UniTask<HealingCoreInitData> CreateHealingCoreData()
        {
            return await _resourceService.Load<HealingCoreInitData>(HealingCoreData);
        }

        public async UniTask<GoldCoreInitData> CreateGoldCoreData()
        {
            return await _resourceService.Load<GoldCoreInitData>(GoldCoreData);
        }

        public async UniTask<AlienCocoonInitData> CreateAlienCocoonData()
        {
            return await _resourceService.Load<AlienCocoonInitData>(AlienCocoonData);
        }

        public async UniTask<IceCrystalInitData> CreateIceCrystalData()
        {
            return await _resourceService.Load<IceCrystalInitData>(IceCrystalData);
        }
    }
}
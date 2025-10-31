using System;
using Cysharp.Threading.Tasks;
using Project.Scripts.Services;
using Reflex.Attributes;
using UnityEngine;
using UnityEngine.UI;
using YG;

namespace Project.Scripts.Weapon.Player
{
    public class WeaponFactory : MonoBehaviour
    {
        private readonly string _enemyDetectorPath = "EnemyDetectorForPlayer";
        private readonly string _gunPath = "Gun";
        private readonly string _fourBarrelMachineGunPath = "FourBarrelMachineGun";
        private readonly string _minesPath = "Mines";
        private readonly string _fragGrenadesPath = "FragGrenades";
        private readonly string _machineGunPath = "MachineGun";
        private readonly string _chainLightningGunPath = "ChainLightningGun";

        private AudioSoundsService _audioSoundsService;
        private IResourceService _resourceService;
        private ICharacteristicsWeaponDataService _characteristicsWeaponDataService;
        private ILevelUpService _levelUpService;

        private EnemyDetectorForPlayer _enemyDetector;
        private WeaponHolder _weaponHolder;
        private Button _minesButton;
        private Transform _player;

        private int _weaponsCounter;

        public event Action<int, WeaponType> WeaponIsCreated;
        public event Action MinesIsCreated;

        [Inject]
        private void Construct(AudioSoundsService audioSoundsService, IResourceService resourceService,
            ICharacteristicsWeaponDataService characteristicsWeaponDataService, ILevelUpService levelUpService)
        {
            _audioSoundsService = audioSoundsService;
            _resourceService = resourceService;
            _characteristicsWeaponDataService = characteristicsWeaponDataService;
            _levelUpService = levelUpService;
        }

        public async UniTask<PlayerWeapon> CreateWeapon(WeaponType weaponType)
        {
            WeaponIsCreated?.Invoke(_weaponsCounter, weaponType);
            _weaponsCounter++;

            _levelUpService.RemoveWeaponCard(weaponType);

            switch (weaponType)
            {
                case WeaponType.Gun:
                    return await CreateGun();
                case WeaponType.MachineGun:
                    return await CreateMachineGun();
                case WeaponType.Mines:
                    return await CreateMines();
                case WeaponType.FragGrenades:
                    return await CreateFragGrenades();
                case WeaponType.FourBarrelMachineGun:
                    return await CreateFourBarrelMachineGun();
                case WeaponType.ChainLightningGun:
                    return await CreateChainLightningGun();
                default:
                    return null;
            }
        }

        public void GetData(Transform player, WeaponHolder weaponHolder)
        {
            _weaponHolder = weaponHolder;
            _player = player;
        }

        public void GetMinesButton(Button button)
        {
            _minesButton = button;
        }

        public async UniTask CreateEnemyDetectorForPlayer()
        {
            var enemyDetectorTemplate = await _resourceService.Load<GameObject>(_enemyDetectorPath);
            enemyDetectorTemplate = Instantiate(enemyDetectorTemplate);

            _enemyDetector = enemyDetectorTemplate.GetComponent<EnemyDetectorForPlayer>();
            _enemyDetector.Construct(_player);
        }

        private async UniTask<PlayerWeapon> CreateGun()
        {
            var gunTemplate = await _resourceService.Load<GameObject>(_gunPath);
            gunTemplate = Instantiate(gunTemplate, _player);

            Gun gun = gunTemplate.GetComponent<Gun>();

            var gunCharacteristics = YG2.saves.GunCharacteristics;
            var gunCharacteristicsData = _characteristicsWeaponDataService.GetWeaponDataByType(WeaponType.Gun);

            gun.Construct(_enemyDetector, _audioSoundsService, gunCharacteristicsData, gunCharacteristics);
            _weaponHolder.AddWeapon(gun);

            return gun;
        }

        private async UniTask<PlayerWeapon> CreateFourBarrelMachineGun()
        {
            var fourBarrelMachineGunTemplate = await _resourceService.Load<GameObject>(_fourBarrelMachineGunPath);
            fourBarrelMachineGunTemplate = Instantiate(fourBarrelMachineGunTemplate, _player);

            FourBarrelMachineGun fourBarrelMachineGun =
                fourBarrelMachineGunTemplate.GetComponent<FourBarrelMachineGun>();

            var fourBarrelMachineGunCharacteristics = YG2.saves.FourBarrelMachineGunCharacteristics;
            var fourBarrelMachineGunData =
                _characteristicsWeaponDataService.GetWeaponDataByType(WeaponType.FourBarrelMachineGun);

            fourBarrelMachineGun.Construct(_audioSoundsService, _enemyDetector, fourBarrelMachineGunData,
                fourBarrelMachineGunCharacteristics);
            _weaponHolder.AddWeapon(fourBarrelMachineGun);

            return fourBarrelMachineGun;
        }

        private async UniTask<PlayerWeapon> CreateMines()
        {
            Vector3 position = new Vector3(_player.position.x, 0f, _player.position.z);

            var minesTemplate = await _resourceService.Load<GameObject>(_minesPath);
            minesTemplate = Instantiate(minesTemplate, _player);

            Mines mines = minesTemplate.GetComponent<Mines>();

            var minesCharacteristics = YG2.saves.MinesCharacteristics;
            var minesData = _characteristicsWeaponDataService.GetWeaponDataByType(WeaponType.Mines);

            mines.transform.position = position;
            mines.Construct(_minesButton, _audioSoundsService, minesData, minesCharacteristics);
            _weaponHolder.AddWeapon(mines);

            MinesIsCreated?.Invoke();

            return mines;
        }

        private async UniTask<PlayerWeapon> CreateFragGrenades()
        {
            var fragGrenadesTemplate = await _resourceService.Load<GameObject>(_fragGrenadesPath);
            fragGrenadesTemplate = Instantiate(fragGrenadesTemplate, _player);

            FragGrenades fragGrenades = fragGrenadesTemplate.GetComponent<FragGrenades>();

            var fragGrenadesCharacteristics = YG2.saves.FragGrenadeCharacteristics;
            var fragGrenadesData = _characteristicsWeaponDataService.GetWeaponDataByType(WeaponType.FragGrenades);

            fragGrenades.Construct(_enemyDetector, _audioSoundsService, fragGrenadesData, fragGrenadesCharacteristics);
            _weaponHolder.AddWeapon(fragGrenades);

            return fragGrenades;
        }

        private async UniTask<PlayerWeapon> CreateMachineGun()
        {
            var machineGunTemplate = await _resourceService.Load<GameObject>(_machineGunPath);
            machineGunTemplate = Instantiate(machineGunTemplate, _player);

            MachineGun machineGun = machineGunTemplate.GetComponent<MachineGun>();

            var machineGunCharacteristics = YG2.saves.MachineGunCharacteristics;
            var machineGunData = _characteristicsWeaponDataService.GetWeaponDataByType(WeaponType.MachineGun);

            machineGun.Construct(_enemyDetector, _audioSoundsService, machineGunData, machineGunCharacteristics);
            _weaponHolder.AddWeapon(machineGun);

            return machineGun;
        }

        private async UniTask<PlayerWeapon> CreateChainLightningGun()
        {
            var chainLightningGunTemplate = await _resourceService.Load<GameObject>(_chainLightningGunPath);
            chainLightningGunTemplate = Instantiate(chainLightningGunTemplate, _player);

            ChainLightningGun chainLightningGun = chainLightningGunTemplate.GetComponent<ChainLightningGun>();

            var chainLightningGunCharacteristics = YG2.saves.ChainLightningGunCharacteristics;
            var chainLightningGunData =
                _characteristicsWeaponDataService.GetWeaponDataByType(WeaponType.ChainLightningGun);

            chainLightningGun.Construct(_audioSoundsService, _enemyDetector, chainLightningGunData, 
                chainLightningGunCharacteristics);
            _weaponHolder.AddWeapon(chainLightningGun);

            return chainLightningGun;
        }
    }
}
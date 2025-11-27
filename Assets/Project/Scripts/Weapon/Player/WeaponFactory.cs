using System;
using Cysharp.Threading.Tasks;
using Project.Scripts.Services;
using Project.Scripts.UI.Panel;
using Reflex.Attributes;
using UnityEngine;
using UnityEngine.UI;
using YG;

namespace Project.Scripts.Weapon.Player
{
    public class WeaponFactory : MonoBehaviour
    {
        private const string EnemyDetectorPath = "EnemyDetectorForPlayer";
        private const string GunPath = "Gun";
        private const string FourBarrelMachineGunPath = "FourBarrelMachineGun";
        private const string MinesPath = "Mines";
        private const string FragGrenadesPath = "FragGrenades";
        private const string MachineGunPath = "MachineGun";
        private const string ChainLightningGunPath = "ChainLightningGun";

        private const float Height = 0f;

        private AudioSoundsService _audioSoundsService;
        private ParticleEffectsService _particleEffectsService;
        private IResourceService _resourceService;
        private ICharacteristicsWeaponDataService _characteristicsWeaponDataService;
        private ILevelUpService _levelUpService;
        private IPlayerService _playerService;

        private EnemyDetectorForPlayer _enemyDetector;
        private WeaponHolder _weaponHolder;
        private WeaponPanel _weaponPanel;
        private Button _minesButton;
        private Transform _player;
        
        private Mines _mines;

        private int _weaponsCounter;

        public event Action<int, WeaponType> WeaponIsCreated;
        public event Action MinesIsCreated;

        [Inject]
        private void Construct(AudioSoundsService audioSoundsService, IResourceService resourceService,
            ICharacteristicsWeaponDataService characteristicsWeaponDataService, ILevelUpService levelUpService,
            ParticleEffectsService particleEffectsService, IPlayerService playerService)
        {
            _audioSoundsService = audioSoundsService;
            _particleEffectsService = particleEffectsService;
            _resourceService = resourceService;
            _characteristicsWeaponDataService = characteristicsWeaponDataService;
            _levelUpService = levelUpService;
            _playerService = playerService;
        }

        private void OnDestroy()
        {
            if(_mines != null)
                _playerService.PlayerActor.PlayerInputController.OnWeaponButtonPressed -= _mines.Shoot;
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

        public void GetData(Transform player, WeaponHolder weaponHolder, WeaponPanel weaponPanel)
        {
            _weaponHolder = weaponHolder;
            _weaponPanel = weaponPanel;
            _player = player;
        }

        public void GetMinesButton(Button button)
        {
            _minesButton = button;
        }

        public async UniTask CreateEnemyDetectorForPlayer()
        {
            var enemyDetectorTemplate = await _resourceService.Load<GameObject>(EnemyDetectorPath);
            enemyDetectorTemplate = Instantiate(enemyDetectorTemplate);

            _enemyDetector = enemyDetectorTemplate.GetComponent<EnemyDetectorForPlayer>();
            _enemyDetector.Construct(_player);
        }

        private async UniTask<PlayerWeapon> CreateGun()
        {
            var gunTemplate = await _resourceService.Load<GameObject>(GunPath);
            gunTemplate = Instantiate(gunTemplate, _player);

            Gun gun = gunTemplate.GetComponent<Gun>();

            var gunCharacteristics = YG2.saves.GunCharacteristics;
            var gunCharacteristicsData = _characteristicsWeaponDataService.GetWeaponDataByType(WeaponType.Gun);

            gun.Construct(_enemyDetector, _audioSoundsService, gunCharacteristicsData, gunCharacteristics, _weaponPanel);
            _weaponHolder.AddWeapon(gun);

            return gun;
        }

        private async UniTask<PlayerWeapon> CreateFourBarrelMachineGun()
        {
            var fourBarrelMachineGunTemplate = await _resourceService.Load<GameObject>(FourBarrelMachineGunPath);
            fourBarrelMachineGunTemplate = Instantiate(fourBarrelMachineGunTemplate, _player);

            FourBarrelMachineGun fourBarrelMachineGun =
                fourBarrelMachineGunTemplate.GetComponent<FourBarrelMachineGun>();

            var fourBarrelMachineGunCharacteristics = YG2.saves.FourBarrelMachineGunCharacteristics;
            var fourBarrelMachineGunData =
                _characteristicsWeaponDataService.GetWeaponDataByType(WeaponType.FourBarrelMachineGun);

            fourBarrelMachineGun.Construct(_audioSoundsService, _enemyDetector, fourBarrelMachineGunData,
                fourBarrelMachineGunCharacteristics, _weaponPanel);
            _weaponHolder.AddWeapon(fourBarrelMachineGun);

            return fourBarrelMachineGun;
        }

        private async UniTask<PlayerWeapon> CreateMines()
        {
            Vector3 position = new Vector3(_player.position.x, Height, _player.position.z);

            var minesTemplate = await _resourceService.Load<GameObject>(MinesPath);
            minesTemplate = Instantiate(minesTemplate, _player);

            Mines mines = minesTemplate.GetComponent<Mines>();

            var minesCharacteristics = YG2.saves.MinesCharacteristics;
            var minesData = _characteristicsWeaponDataService.GetWeaponDataByType(WeaponType.Mines);

            mines.transform.position = position;
            mines.Construct(_minesButton, _audioSoundsService, minesData, minesCharacteristics,
                _particleEffectsService, _weaponPanel);
            _weaponHolder.AddWeapon(mines);

            MinesIsCreated?.Invoke();

            _mines = mines;
            _playerService.PlayerActor.PlayerInputController.OnWeaponButtonPressed += _mines.Shoot;

            return mines;
        }

        private async UniTask<PlayerWeapon> CreateFragGrenades()
        {
            var fragGrenadesTemplate = await _resourceService.Load<GameObject>(FragGrenadesPath);
            fragGrenadesTemplate = Instantiate(fragGrenadesTemplate, _player);

            FragGrenades fragGrenades = fragGrenadesTemplate.GetComponent<FragGrenades>();

            var fragGrenadesCharacteristics = YG2.saves.FragGrenadeCharacteristics;
            var fragGrenadesData = _characteristicsWeaponDataService.GetWeaponDataByType(WeaponType.FragGrenades);

            fragGrenades.Construct(_enemyDetector, _audioSoundsService, fragGrenadesData, fragGrenadesCharacteristics,
                _particleEffectsService);
            _weaponHolder.AddWeapon(fragGrenades);

            return fragGrenades;
        }

        private async UniTask<PlayerWeapon> CreateMachineGun()
        {
            var machineGunTemplate = await _resourceService.Load<GameObject>(MachineGunPath);
            machineGunTemplate = Instantiate(machineGunTemplate, _player);

            MachineGun machineGun = machineGunTemplate.GetComponent<MachineGun>();

            var machineGunCharacteristics = YG2.saves.MachineGunCharacteristics;
            var machineGunData = _characteristicsWeaponDataService.GetWeaponDataByType(WeaponType.MachineGun);

            machineGun.Construct(_enemyDetector, _audioSoundsService, machineGunData, machineGunCharacteristics
            , _weaponPanel);
            _weaponHolder.AddWeapon(machineGun);

            return machineGun;
        }

        private async UniTask<PlayerWeapon> CreateChainLightningGun()
        {
            var chainLightningGunTemplate = await _resourceService.Load<GameObject>(ChainLightningGunPath);
            chainLightningGunTemplate = Instantiate(chainLightningGunTemplate, _player);

            ChainLightningGun chainLightningGun = chainLightningGunTemplate.GetComponent<ChainLightningGun>();

            var chainLightningGunCharacteristics = YG2.saves.ChainLightningGunCharacteristics;
            var chainLightningGunData =
                _characteristicsWeaponDataService.GetWeaponDataByType(WeaponType.ChainLightningGun);

            chainLightningGun.Construct(_audioSoundsService, _enemyDetector, chainLightningGunData, 
                chainLightningGunCharacteristics, _weaponPanel);
            _weaponHolder.AddWeapon(chainLightningGun);

            return chainLightningGun;
        }
    }
}
using System;
using Cysharp.Threading.Tasks;
using Project.Scripts.DataBase.Data;
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

        private EnemyDetectorForPlayer _enemyDetector;
        private WeaponHolder _weaponHolder;
        private Button _minesButton;
        private Transform _player;

        private int _weaponsCounter;

        private CharacteristicsWeaponData _gunData;
        private CharacteristicsWeaponData _machineGunData;
        private CharacteristicsWeaponData _minesData;
        private CharacteristicsWeaponData _fragGrenadesData;
        private CharacteristicsWeaponData _fourBarrelMachineGunData;
        private CharacteristicsWeaponData _chainLightningGunData;

        public event Action<int, WeaponType> WeaponIsCreated;
        public event Action MinesIsCreated;

        [Inject]
        private void Construct(AudioSoundsService audioSoundsService, IResourceService resourceService,
            ICharacteristicsWeaponDataService characteristicsWeaponDataService)
        {
            _audioSoundsService = audioSoundsService;
            _resourceService = resourceService;
            _characteristicsWeaponDataService = characteristicsWeaponDataService;
        }

        private void Start()
        {
            _gunData = YG2.saves.GunCharacteristics;
            _machineGunData = YG2.saves.MachineGunCharacteristics;
            _minesData = YG2.saves.MinesCharacteristics;
            _fragGrenadesData = YG2.saves.FragGrenadeCharacteristics;
            _fourBarrelMachineGunData = YG2.saves.FourBarrelMachineGunCharacteristics;
            _chainLightningGunData = YG2.saves.ChainLightningGunCharacteristics;
        }

        public async UniTask<PlayerWeapon> CreateWeapon(WeaponType weaponType)
        {
            WeaponIsCreated?.Invoke(_weaponsCounter, weaponType);
            _weaponsCounter++;

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
            
            if (_gunData == null)
            {
                _gunData = _characteristicsWeaponDataService.GetWeaponDataByType(WeaponType.Gun);
                YG2.saves.GunCharacteristics = _gunData;
            }
            
            gun.Construct(_enemyDetector, _audioSoundsService, _gunData);
            _weaponHolder.AddWeapon(gun);
            
            return gun;
        }

        private async UniTask<PlayerWeapon> CreateFourBarrelMachineGun()
        {
            var fourBarrelMachineGunTemplate = await _resourceService.Load<GameObject>(_fourBarrelMachineGunPath);
            fourBarrelMachineGunTemplate = Instantiate(fourBarrelMachineGunTemplate, _player);

            FourBarrelMachineGun fourBarrelMachineGun = 
                fourBarrelMachineGunTemplate.GetComponent<FourBarrelMachineGun>();
            
            if (_fourBarrelMachineGunData == null)
            {
                _fourBarrelMachineGunData = 
                    _characteristicsWeaponDataService.GetWeaponDataByType(WeaponType.FourBarrelMachineGun);
                YG2.saves.FourBarrelMachineGunCharacteristics = _fourBarrelMachineGunData;
            }
            
            fourBarrelMachineGun.Construct(_audioSoundsService, _enemyDetector, _fourBarrelMachineGunData);
            _weaponHolder.AddWeapon(fourBarrelMachineGun);
            
            return fourBarrelMachineGun;
        }

        private async UniTask<PlayerWeapon> CreateMines()
        {
            Vector3 position = new Vector3(_player.position.x, 0f, _player.position.z);

            var minesTemplate = await _resourceService.Load<GameObject>(_minesPath);
            minesTemplate = Instantiate(minesTemplate, _player);

            Mines mines = minesTemplate.GetComponent<Mines>();

            if (_minesData == null)
            {
                _minesData = _characteristicsWeaponDataService.GetWeaponDataByType(WeaponType.Mines);
                YG2.saves.MinesCharacteristics = _minesData;
            }
            
            mines.transform.position = position;
            mines.Construct(_minesButton, _audioSoundsService, _minesData);
            _weaponHolder.AddWeapon(mines);

            MinesIsCreated?.Invoke();

            return mines;
        }

        private async UniTask<PlayerWeapon> CreateFragGrenades()
        {
            var fragGrenadesTemplate = await _resourceService.Load<GameObject>(_fragGrenadesPath);
            fragGrenadesTemplate = Instantiate(fragGrenadesTemplate, _player);

            FragGrenades fragGrenades = fragGrenadesTemplate.GetComponent<FragGrenades>();

            if (_fragGrenadesData == null)
            {
                _fragGrenadesData = _characteristicsWeaponDataService.GetWeaponDataByType(WeaponType.FragGrenades);
                YG2.saves.FragGrenadeCharacteristics = _fragGrenadesData;
            }
            
            fragGrenades.Construct(_enemyDetector, _audioSoundsService, _fragGrenadesData);
            _weaponHolder.AddWeapon(fragGrenades);

            return fragGrenades;
        }

        private async UniTask<PlayerWeapon> CreateMachineGun()
        {
            var machineGunTemplate = await _resourceService.Load<GameObject>(_machineGunPath);
            machineGunTemplate = Instantiate(machineGunTemplate, _player);

            MachineGun machineGun = machineGunTemplate.GetComponent<MachineGun>();

            if (_machineGunData == null)
            {
                _machineGunData = _characteristicsWeaponDataService.GetWeaponDataByType(WeaponType.MachineGun);
                YG2.saves.MachineGunCharacteristics = _machineGunData;
            }
            
            machineGun.Construct(_enemyDetector, _audioSoundsService, _machineGunData);
            _weaponHolder.AddWeapon(machineGun);

            return machineGun;
        }

        private async UniTask<PlayerWeapon> CreateChainLightningGun()
        {
            var chainLightningGunTemplate = await _resourceService.Load<GameObject>(_chainLightningGunPath);
            chainLightningGunTemplate = Instantiate(chainLightningGunTemplate, _player);

            ChainLightningGun chainLightningGun = chainLightningGunTemplate.GetComponent<ChainLightningGun>();
            
            if (_chainLightningGunData == null)
            {
                _chainLightningGunData = 
                    _characteristicsWeaponDataService.GetWeaponDataByType(WeaponType.ChainLightningGun);
                YG2.saves.ChainLightningGunCharacteristics = _chainLightningGunData;
            }
            
            chainLightningGun.Construct(_audioSoundsService, _enemyDetector, _chainLightningGunData);
            _weaponHolder.AddWeapon(chainLightningGun);

            return chainLightningGun;
        }
    }
}
using System;
using Cysharp.Threading.Tasks;
using Project.Scripts.Services;
using Reflex.Attributes;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.Weapon.Player
{
    public class WeaponFactory : MonoBehaviour
    {
        private readonly string _enemyDetectorPath = "EnemyDetector";
        private readonly string _newEnemyDetectorPath = "NewEnemyDetector";
        private readonly string _gunPath = "Gun";
        private readonly string _fourBarrelMachineGunPath = "FourBarrelMachineGun";
        private readonly string _minesPath = "Mines";
        private readonly string _fragGrenadesPath = "FragGrenades";
        private readonly string _machineGunPath = "MachineGun";
        private readonly string _chainLightningGunPath = "ChainLightningGun";

        private AudioSoundsService _audioSoundsService;
        private IResourceService _resourceService;

        private EnemyDetector _enemyDetector;
        private NewEnemyDetector _newEnemyDetector;
        private WeaponHolder _weaponHolder;
        private Button _minesButton;
        private Transform _player;

        public event Action MinesIsCreated;

        [Inject]
        private void Construct(AudioSoundsService audioSoundsService, IResourceService resourceService)
        {
            _audioSoundsService = audioSoundsService;
            _resourceService = resourceService;
        }

        public async UniTask<PlayerWeapon> CreateWeapon(WeaponType weaponType)
        {
            switch (weaponType)
            {
                case WeaponType.Gun :
                    return await CreateGun();
                case WeaponType.MachineGun :
                    return await CreateMachineGun();
                case WeaponType.Mines :
                    return await CreateMines();
                case WeaponType.FragGrenades : 
                    return await CreateFragGrenades();
                case WeaponType.FourBarrelMachineGun :
                    return await CreateFourBarrelMachineGun();
                case WeaponType.ChainLightningGun :
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

        public async UniTask CreateEnemyDetector()
        {
            var enemyDetectorTemplate = await _resourceService.Load<GameObject>(_enemyDetectorPath);
            enemyDetectorTemplate = Instantiate(enemyDetectorTemplate, _player);
            _enemyDetector = enemyDetectorTemplate.GetComponent<EnemyDetector>();
        }
        
        public async UniTask CreateNewEnemyDetector()
        {
            var enemyDetectorTemplate = await _resourceService.Load<GameObject>(_newEnemyDetectorPath);
            enemyDetectorTemplate = Instantiate(enemyDetectorTemplate, _player);
            _newEnemyDetector = enemyDetectorTemplate.GetComponent<NewEnemyDetector>();
        }
        
        private async UniTask<PlayerWeapon> CreateGun()
        {
            var gunTemplate = await _resourceService.Load<GameObject>(_gunPath);
            gunTemplate = Instantiate(gunTemplate, _player);
            
            Gun gun = gunTemplate.GetComponent<Gun>();
            gun.Construct(_enemyDetector, _audioSoundsService);
            _weaponHolder.AddWeapon(gun);

            return gun;
        }
        
        private async UniTask<PlayerWeapon> CreateFourBarrelMachineGun()
        {
            var fourBarrelMachineGunTemplate = await _resourceService.Load<GameObject>(_fourBarrelMachineGunPath);
            fourBarrelMachineGunTemplate = Instantiate(fourBarrelMachineGunTemplate, _player);

            FourBarrelMachineGun fourBarrelMachineGun = fourBarrelMachineGunTemplate.GetComponent<FourBarrelMachineGun>();
            fourBarrelMachineGun.Construct(_audioSoundsService);
            _weaponHolder.AddWeapon(fourBarrelMachineGun);

            return fourBarrelMachineGun;
        }

        private async UniTask<PlayerWeapon> CreateMines()
        {
            Vector3 position = new Vector3(_player.position.x, 0f, _player.position.z);
            
            var minesTemplate = await _resourceService.Load<GameObject>(_minesPath);
            minesTemplate = Instantiate(minesTemplate, _player);

            Mines mines = minesTemplate.GetComponent<Mines>();
            mines.transform.position = position;
            mines.Construct(_minesButton, _audioSoundsService);
            _weaponHolder.AddWeapon(mines);
            
            MinesIsCreated?.Invoke();

            return mines;
        }

        private async UniTask<PlayerWeapon> CreateFragGrenades()
        {
            var fragGrenadesTemplate = await _resourceService.Load<GameObject>(_fragGrenadesPath);
            fragGrenadesTemplate = Instantiate(fragGrenadesTemplate, _player);

            FragGrenades fragGrenades = fragGrenadesTemplate.GetComponent<FragGrenades>();
            fragGrenades.Construct(_enemyDetector, _audioSoundsService);
            _weaponHolder.AddWeapon(fragGrenades);

            return fragGrenades;
        }

        private async UniTask<PlayerWeapon> CreateMachineGun()
        {
            var machineGunTemplate = await _resourceService.Load<GameObject>(_machineGunPath);
            machineGunTemplate = Instantiate(machineGunTemplate, _player);

            MachineGun machineGun = machineGunTemplate.GetComponent<MachineGun>();
            machineGun.Construct(_enemyDetector, _audioSoundsService);
            _weaponHolder.AddWeapon(machineGun);

            return machineGun;
        }

        private async UniTask<PlayerWeapon> CreateChainLightningGun()
        {
            var chainLightningGunTemplate = await _resourceService.Load<GameObject>(_chainLightningGunPath);
            chainLightningGunTemplate = Instantiate(chainLightningGunTemplate, _player);

            ChainLightningGun chainLightningGun = chainLightningGunTemplate.GetComponent<ChainLightningGun>();
            chainLightningGun.Construct(_audioSoundsService, _newEnemyDetector);
            _weaponHolder.AddWeapon(chainLightningGun);

            return chainLightningGun;
        }
    }
}
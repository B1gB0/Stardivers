using System;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Game.Scripts
{
    public class WeaponFactory : MonoBehaviour
    {
        private const string Gun = nameof(Gun);
        private const string MachineGun = nameof(MachineGun);
        private const string Mines = nameof(Mines);
        private const string FragGrenades = nameof(FragGrenades);
        private const string FourBarrelMachineGun = nameof(FourBarrelMachineGun);

        [SerializeField] private ClosestEnemyDetector _enemyDetectorTemplate;
        [SerializeField] private Gun _gunTemplate;
        [SerializeField] private MachineGun _machineGunTemplate;
        [SerializeField] private Mines _minesTemplate;
        [SerializeField] private FragGrenades _fragGrenadesTemplate;
        [SerializeField] private FourBarrelMachineGun _fourBarrelMachineGunTemplate;

        private AudioSoundsService _audioSoundsService;
        private ClosestEnemyDetector _enemyDetector;
        private WeaponHolder _weaponHolder;
        private Button _minesButton;
        private Transform _player;

        public event Action MinesIsCreated;

        public void CreateWeapon(string weaponType)
        {
            switch (weaponType)
            {
                case Gun :
                    CreateGun();
                    break;
                case MachineGun :
                    CreateMachineGun();
                    break;
                case Mines :
                    CreateMines();
                    break;
                case FragGrenades : 
                    CreateFragGrenades();
                    break;
                case FourBarrelMachineGun :
                    CreateFourBarrelMachineGun();
                    break;
            }
        }

        public void GetData(Transform player, WeaponHolder weaponHolder, AudioSoundsService audioSoundsService)
        {
            _weaponHolder = weaponHolder;
            _player = player;
            _audioSoundsService = audioSoundsService;
        }

        public void GetMinesButton(Button button)
        {
            _minesButton = button;
        }

        public void CreateEnemyDetector()
        {
            _enemyDetector = Instantiate(_enemyDetectorTemplate, _player);
        }
        
        private void CreateGun()
        {
            Gun gun = Instantiate(_gunTemplate, _player);
            gun.Construct(_enemyDetector, _audioSoundsService);
            _weaponHolder.AddWeapon(gun);
        }
        
        private void CreateMachineGun()
        {
            MachineGun machineGun = Instantiate(_machineGunTemplate, _player);
            machineGun.Construct(_enemyDetector, _audioSoundsService);
            _weaponHolder.AddWeapon(machineGun);
        }

        private void CreateMines()
        {
            MinesIsCreated?.Invoke();
            
            Vector3 position = new Vector3(_player.position.x, 0f, _player.position.z);
            Mines mines = Instantiate(_minesTemplate, _player);
            mines.transform.position = position;
            mines.Construct(_minesButton, _audioSoundsService);
            _weaponHolder.AddWeapon(mines);
        }

        private void CreateFragGrenades()
        {
            FragGrenades fragGrenades = Instantiate(_fragGrenadesTemplate, _player);
            fragGrenades.Construct(_enemyDetector, _audioSoundsService);
            _weaponHolder.AddWeapon(fragGrenades);
        }

        private void CreateFourBarrelMachineGun()
        {
            FourBarrelMachineGun fourBarrelMachineGun = Instantiate(_fourBarrelMachineGunTemplate, _player);
            fourBarrelMachineGun.Construct(_audioSoundsService);
            _weaponHolder.AddWeapon(fourBarrelMachineGun);
        }
    }
}
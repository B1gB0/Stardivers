using System;
using Project.Scripts.Services;
using Reflex.Attributes;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.Weapon.Player
{
    public class WeaponFactory : MonoBehaviour
    {
        [SerializeField] private EnemyDetector _enemyDetectorTemplate;
        [SerializeField] private Gun _gunTemplate;
        [SerializeField] private FourBarrelMachineGun fourBarrelMachineGunTemplate;
        [SerializeField] private Mines _minesTemplate;
        [SerializeField] private FragGrenades _fragGrenadesTemplate;
        [SerializeField] private MachineGun machineGunTemplate;

        private AudioSoundsService _audioSoundsService;
        private EnemyDetector _enemyDetector;
        private WeaponHolder _weaponHolder;
        private Button _minesButton;
        private Transform _player;

        public event Action MinesIsCreated;

        [Inject]
        private void Construct(AudioSoundsService audioSoundsService)
        {
            _audioSoundsService = audioSoundsService;
        }

        public void CreateWeapon(WeaponType weaponType)
        {
            switch (weaponType)
            {
                case WeaponType.Gun :
                    CreateGun();
                    break;
                case WeaponType.MachineGun :
                    CreateMachineGun();
                    break;
                case WeaponType.Mines :
                    CreateMines();
                    break;
                case WeaponType.FragGrenades : 
                    CreateFragGrenades();
                    break;
                case WeaponType.FourBarrelMachineGun :
                    CreateFourBarrelMachineGun();
                    break;
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
        
        private void CreateFourBarrelMachineGun()
        {
            FourBarrelMachineGun fourBarrelMachineGun = Instantiate(fourBarrelMachineGunTemplate, _player);
            fourBarrelMachineGun.Construct(_audioSoundsService);
            _weaponHolder.AddWeapon(fourBarrelMachineGun);
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

        private void CreateMachineGun()
        {
            MachineGun machineGun = Instantiate(machineGunTemplate, _player);
            machineGun.Construct(_enemyDetector, _audioSoundsService);
            _weaponHolder.AddWeapon(machineGun);
        }
    }
}
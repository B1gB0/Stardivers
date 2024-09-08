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

        [SerializeField] private ClosestEnemyDetector _enemyDetectorTemplate;
        [SerializeField] private Gun _gunTemplate;
        [SerializeField] private MachineGun _machineGunTemplate;
        [SerializeField] private Mines _minesTemplate;
        [SerializeField] private FragGrenades _fragGrenadesTemplate;

        private ClosestEnemyDetector _enemyDetector;
        private WeaponHolder _weaponHolder;
        private Button _minesButton;
        private Transform _player;

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
            gun.Construct(_enemyDetector);
            _weaponHolder.AddWeapon(gun);
        }
        
        private void CreateMachineGun()
        {
            MachineGun machineGun = Instantiate(_machineGunTemplate, _player);
            machineGun.Construct(_enemyDetector);
            _weaponHolder.AddWeapon(machineGun);
        }

        private void CreateMines()
        {
            Vector3 position = new Vector3(_player.position.x, 0f, _player.position.z);
            Mines mines = Instantiate(_minesTemplate, _player);
            mines.transform.position = position;
            mines.Construct(_minesButton);
            _weaponHolder.AddWeapon(mines);
        }

        private void CreateFragGrenades()
        {
            FragGrenades fragGrenades = Instantiate(_fragGrenadesTemplate, _player);
            fragGrenades.Construct(_enemyDetector);
            _weaponHolder.AddWeapon(fragGrenades);
        }
    }
}
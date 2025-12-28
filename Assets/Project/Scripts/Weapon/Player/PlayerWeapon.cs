using Cysharp.Threading.Tasks;
using Project.Scripts.UI.Panel;
using Project.Scripts.UI.View;
using Project.Scripts.Weapon.CharacteristicsOfWeapon;
using Project.Scripts.Weapon.Improvements;
using UnityEngine;

namespace Project.Scripts.Weapon.Player
{
    public abstract class PlayerWeapon : MonoBehaviour
    {
        protected const float MinValue = 0f;
        protected const int MinCountShots = 0;
        
        protected WeaponPanel WeaponPanel;
        protected WeaponCharacteristics WeaponCharacteristics;
        protected WeaponView WeaponView;
        
        protected bool IsReloading;
        protected int CurrentCountShots;
        protected float LastShotTime;
        
        private float _reloadTimer;
        
        public WeaponType Type { get; protected set; }

        public abstract void Shoot();

        public abstract void AcceptWeaponImprovement(IWeaponVisitor weaponVisitor, CharacteristicType type,
            float value);
        
        protected void CheckAmmoAndReload()
        {
            if (CurrentCountShots <= MinCountShots && !IsReloading)
            {
                Reload().Forget();
            }
        }

        private async UniTask Reload()
        {
            _reloadTimer = MinValue;
            
            IsReloading = true;
            WeaponView.ActivateFiller();

            while (_reloadTimer < WeaponCharacteristics.ReloadTime)
            {
                if(WeaponView == null)
                    return;
                
                WeaponView.AnimateFiller(_reloadTimer, WeaponCharacteristics.ReloadTime);
                _reloadTimer += Time.deltaTime;
                await UniTask.NextFrame();
            }

            CurrentCountShots = WeaponCharacteristics.MaxCountShots;
            IsReloading = false;
            WeaponView.SetText(CurrentCountShots, WeaponCharacteristics.MaxCountShots);
            WeaponView.DeactivateFiller();
        }
    }
}
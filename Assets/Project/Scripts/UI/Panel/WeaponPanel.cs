using System.Collections.Generic;
using DG.Tweening;
using Project.Scripts.Services;
using Project.Scripts.UI.View;
using Project.Scripts.Weapon.Player;
using Reflex.Attributes;
using UnityEngine;

namespace Project.Scripts.UI.Panel
{
    public class WeaponPanel : MonoBehaviour, IView
    {
        private const int Gun = 0;
        private const int MachineGun = 1;
        private const int FourBarrelMachineGun = 2;
        private const int Grenades = 3;
        private const int Mines = 4;
        private const int ElectricGun = 5;
        
        private readonly Dictionary<WeaponType, WeaponView> _weaponViewsDictionary = new();
        
        [SerializeField] private List<WeaponView> _weaponViews;
        [SerializeField] private List<Sprite> _sprites;
        [SerializeField] private Transform _showPoint;
        [SerializeField] private Transform _hidePoint;

        private ITweenAnimationService _tweenAnimationService;
    
        [Inject]
        private void Construct(ITweenAnimationService tweenAnimationService)
        {
            _tweenAnimationService = tweenAnimationService;
        }

        public void SetData(int numberWeapon, WeaponType type)
        {
            switch (type)
            {
                case WeaponType.Gun :
                    _weaponViews[numberWeapon].SetWeaponData(_sprites[Gun], type);
                    break;
                case WeaponType.MachineGun : 
                    _weaponViews[numberWeapon].SetWeaponData(_sprites[MachineGun], type);
                    break;
                case WeaponType.FourBarrelMachineGun : 
                    _weaponViews[numberWeapon].SetWeaponData(_sprites[FourBarrelMachineGun], type);
                    break;
                case WeaponType.FragGrenades : 
                    _weaponViews[numberWeapon].SetWeaponData(_sprites[Grenades], type);
                    break;
                case WeaponType.Mines : 
                    _weaponViews[numberWeapon].SetWeaponData(_sprites[Mines], type);
                    break;
                case WeaponType.ChainLightningGun : 
                    _weaponViews[numberWeapon].SetWeaponData(_sprites[ElectricGun], type);
                    break;
            }
            
            _weaponViewsDictionary.Add(type, _weaponViews[numberWeapon]);
        }
        
        public void Show()
        {
            gameObject.SetActive(true);
            _tweenAnimationService.AnimateMove(transform, _showPoint, _hidePoint);
        }

        public void Hide()
        {
            _tweenAnimationService.AnimateMove(transform, _showPoint, _hidePoint, true);
        }

        public WeaponView GetWeaponViewByType(WeaponType type)
        {
            return _weaponViewsDictionary[type];
        }

        private void OnDestroy()
        {
            transform.DOKill();
        }
    }
}
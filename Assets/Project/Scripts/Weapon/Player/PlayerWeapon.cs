using System.Collections;
using Project.Scripts.UI.Panel;
using Project.Scripts.Weapon.CharacteristicsOfWeapon;
using Project.Scripts.Weapon.Improvements;
using UnityEngine;

namespace Project.Scripts.Weapon.Player
{
    public abstract class PlayerWeapon : MonoBehaviour
    {
        public WeaponType Type { get; protected set; }

        protected WeaponPanel WeaponPanel;

        public abstract void Shoot();

        public abstract void AcceptWeaponImprovement(IWeaponVisitor weaponVisitor, CharacteristicType type,
            float value);
    }
}
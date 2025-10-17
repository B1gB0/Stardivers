using Project.Scripts.Weapon.CharacteristicsOfWeapon;
using Project.Scripts.Weapon.Improvements;
using UnityEngine;

namespace Project.Scripts.Weapon.Player
{
    public abstract class PlayerWeapon : MonoBehaviour
    {
        public WeaponType Type { get; protected set; }

        public abstract void Shoot();

        public abstract void AcceptWeaponImprovement(IWeaponVisitor weaponVisitor, CharacteristicType type,
            float value);
    }
}
using Project.Scripts.Weapon.CharacteristicsOfWeapon;
using Project.Scripts.Weapon.Improvements;
using UnityEngine;

namespace Project.Scripts.Weapon.Player
{
    public abstract class PlayerWeapon : MonoBehaviour
    {
        [field: SerializeField] public WeaponType Type { get; private set; }

        public abstract void Shoot();

        public abstract void AcceptWeaponImprovement(IWeaponVisitor weaponVisitor, CharacteristicType type,
            float value);
    }
}
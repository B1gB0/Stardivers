using Project.Game.Scripts;
using Project.Scripts.Weapon.Improvements;
using UnityEngine;

namespace Project.Scripts.Weapon.Player
{
    public abstract class PlayerWeapon : MonoBehaviour
    {
        [field: SerializeField] public Weapons Type { get; private set; }

        public abstract void Shoot();

        public abstract void AcceptWeaponImprovement(IWeaponVisitor weaponVisitor, CharacteristicsTypes type, float value);
    }
}
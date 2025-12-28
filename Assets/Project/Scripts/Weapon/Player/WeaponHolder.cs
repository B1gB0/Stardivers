using System.Collections.Generic;

namespace Project.Scripts.Weapon.Player
{
    public class WeaponHolder
    {
        public List<PlayerWeapon> Weapons { get; } = new ();

        public void AddWeapon(PlayerWeapon playerWeapon)
        {
            Weapons.Add(playerWeapon);
        }
    }
}
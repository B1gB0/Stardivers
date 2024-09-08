using System.Collections.Generic;
using UnityEngine;

namespace Project.Game.Scripts
{
    public class WeaponHolder
    {
        public List<Weapon> Weapons { get; private set; } = new ();

        public void AddWeapon(Weapon weapon)
        {
            Weapons.Add(weapon);
        }
    }
}
using System;
using Project.Game.Scripts;
using Project.Scripts.Weapon;
using Project.Scripts.Weapon.Player;
using UnityEngine;

[CreateAssetMenu(menuName = "Cards/PlayerWeapon Card")]
public class WeaponCard : Card
{
    [field: SerializeField] public String Characteristics { get; private set; }
    [field: SerializeField] public PlayerWeapon PlayerWeapon { get; private set; }
}

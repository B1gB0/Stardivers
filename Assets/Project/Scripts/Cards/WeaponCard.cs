using System;
using Project.Game.Scripts;
using Project.Scripts.Weapon;
using Project.Scripts.Weapon.Player;
using UnityEngine;

[CreateAssetMenu(menuName = "Cards/PlayerWeapon Card")]
public class WeaponCard : Card
{
    [field: SerializeField] public string Characteristics { get; private set; }
    [field: SerializeField] public WeaponType PlayerWeapon { get; private set; }
}

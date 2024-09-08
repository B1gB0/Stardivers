using System;
using Project.Game.Scripts;
using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Weapon Card")]
public class WeaponCard : Card
{
    [field: SerializeField] public String Characteristics { get; private set; }
    
    [field: SerializeField] public Weapon Weapon { get; private set; }
}

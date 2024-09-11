using Project.Game.Scripts;
using Project.Game.Scripts.Improvements;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [field: SerializeField] public Weapons Type { get; private set; }

    public abstract void Shoot();
    
    public abstract void Accept(IWeaponVisitor weaponVisitor, CharacteristicsTypes type, float value);
}
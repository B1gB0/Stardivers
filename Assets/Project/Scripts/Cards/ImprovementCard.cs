using Project.Scripts.Weapon.CharacteristicsOfWeapon;
using Project.Scripts.Weapon.Player;
using UnityEngine;

namespace Project.Scripts.Cards
{
    [CreateAssetMenu(menuName = "Cards/Improvement Card")]
    public class ImprovementCard : Card
    {
        [field: SerializeField] public WeaponType WeaponType { get; private set; }
        [field: SerializeField] public CharacteristicType CharacteristicType { get; private set; }
        [field: SerializeField] public float Value { get; private set; }
    }
}
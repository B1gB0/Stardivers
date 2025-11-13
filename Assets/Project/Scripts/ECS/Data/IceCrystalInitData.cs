using Project.Scripts.Projectiles.Mines;
using UnityEngine;

namespace Project.Scripts.ECS.Data
{
    [CreateAssetMenu(menuName = "InitData/IceCrystalInitData")]
    public class IceCrystalInitData : InitData
    {
        [field: SerializeField] public IceCrystal IceCrystalPrefab { get; private set; }
        [field: SerializeField] public IceCrystal BigIceCrystalPrefab { get; private set; }
    }
}
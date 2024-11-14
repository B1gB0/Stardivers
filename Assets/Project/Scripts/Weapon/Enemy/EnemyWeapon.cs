using Project.Scripts.Projectiles;
using Project.Scripts.Projectiles.Enemy;
using UnityEngine;

namespace Project.Scripts.Weapon.Enemy
{
    public abstract class EnemyWeapon : MonoBehaviour
    {
        public abstract void Shoot();
    }
}
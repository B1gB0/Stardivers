using UnityEngine;

namespace Project.Scripts.Projectiles.Bullets
{
    public class FourBarrelMachineGunBullet : Projectile
    {
        public override void SetDirection(Vector3 direction)
        {
            Direction = direction;
            Transform.forward = direction;
        }
    }
}
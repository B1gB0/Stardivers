using Cysharp.Threading.Tasks;
using Project.Scripts.Audio.Sounds;
using Project.Scripts.Projectiles.Enemy;

namespace Project.Scripts.Weapon.Enemy
{
    public class AlienTurretWeapon : GenericEnemyWeapon<AlienEnemyTurretProjectile>
    {
        public override void Shoot()
        {
            AudioSoundsService.PlaySound(SoundsType.SplashSound).Forget();
            base.Shoot();
        }
    }
}
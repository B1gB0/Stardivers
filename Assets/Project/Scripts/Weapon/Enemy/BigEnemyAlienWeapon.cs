using Cysharp.Threading.Tasks;
using Project.Scripts.Audio.Sounds;
using Project.Scripts.Projectiles.Enemy;

namespace Project.Scripts.Weapon.Enemy
{
    public class BigEnemyAlienWeapon : GenericEnemyWeapon<BigAlienEnemyProjectile>
    {
        public override void Shoot()
        {
            AudioSoundsService.PlaySound(SoundsType.Splash).Forget();
            base.Shoot();
        }
    }
}
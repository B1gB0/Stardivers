using Cysharp.Threading.Tasks;
using Project.Scripts.Audio.Sounds;
using Project.Scripts.Projectiles.Enemy;

namespace Project.Scripts.Weapon.Enemy
{
    public class GunnerEnemyAlienWeapon : GenericEnemyWeapon<GunnerAlienEnemyProjectile>
    {
        public override void Shoot()
        {
            AudioSoundsService.PlaySound(SoundsType.GunnerEnemyAlien).Forget();
            base.Shoot();
        }
    }
}
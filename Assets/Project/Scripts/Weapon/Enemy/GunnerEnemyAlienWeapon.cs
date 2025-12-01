using Cysharp.Threading.Tasks;
using Project.Scripts.Audio.Sounds;
using Project.Scripts.Projectiles.Enemy;
using Project.Scripts.Services;

namespace Project.Scripts.Weapon.Enemy
{
    public class GunnerEnemyAlienWeapon : GenericEnemyWeapon<GunnerAlienEnemyProjectile>
    {
        private AudioSoundsService _audioSoundsService;
        
        public override void Shoot()
        {
            _audioSoundsService.PlaySound(SoundsType.GunnerEnemyAlien).Forget();
            base.Shoot();
        }

        public void GetServices(AudioSoundsService audioSoundsService)
        {
            _audioSoundsService = audioSoundsService;
        }
    }
}
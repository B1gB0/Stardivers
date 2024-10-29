using Project.Scripts.ECS.EntityActors;
using Project.Scripts.Experience;
using Project.Scripts.Projectiles.Bullets;
using Project.Scripts.Score;
using Project.Scripts.UI;
using UnityEngine;
using NotImplementedException = System.NotImplementedException;

namespace Build.Game.Scripts.ECS.EntityActors
{
    public class BigAlienEnemyAlien : EnemyAlienActor, IAcceptable
    {
        [field: SerializeField] public BigEnemyAlienProjectile Projectile { get; private set; }

        private void OnEnable()
        {
            Health.Die += Die;
        }

        private void OnDisable()
        {
            Health.Die -= Die;
        }

        private void Die()
        {
            Health.IsSpawnedDamageText -= DamageTextService.OnChangedDamageText;
            ExperiencePoints.OnKill(this);
            gameObject.SetActive(false);
        }

        public void AcceptScore(IScoreActorVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
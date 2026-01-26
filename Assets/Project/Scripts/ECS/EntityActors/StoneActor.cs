using Project.Scripts.Experience;

namespace Project.Scripts.ECS.EntityActors
{
    public class StoneActor : ResourceActor
    {
        private void OnEnable()
        {
            Health.Die += Die;
            Health.IsDamaged += OnPlayParticleEffect;
        }

        private void OnDisable()
        {
            Health.Die -= Die;
            Health.IsDamaged -= OnPlayParticleEffect;
        }

        private void Die()
        {
            ExperiencePoints.OnKill(this);
            gameObject.SetActive(false);
        }
    }
}
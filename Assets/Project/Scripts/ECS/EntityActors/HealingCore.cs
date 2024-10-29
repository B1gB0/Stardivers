using Project.Scripts.Crystals;
using Project.Scripts.Experience;
using Project.Scripts.MiningResources;
using UnityEngine;

namespace Project.Scripts.ECS.EntityActors
{
    public class HealingCore : ResourceActor, IAcceptable
    {
        [SerializeField] private HealingCrystal _healingCrystalPrefab;

        private CrystalSpawner _crystalSpawner;
        
        private void OnEnable()
        {
            Health.Die += Die;
            Health.IsDamaged += SpawnCrystal;
        }

        private void OnDisable()
        {
            Health.Die -= Die;
            Health.IsDamaged -= SpawnCrystal;
        }

        public void GetCrystalSpawner(CrystalSpawner crystalSpawner)
        {
            _crystalSpawner = crystalSpawner;
        }
        
        public void AcceptScore(IScoreActorVisitor visitor)
        {
            visitor.Visit(this);
        }

        public void AcceptSpawnCrystal(ICrystalsActorVisitor visitor)
        {
            visitor.Visit(this, _healingCrystalPrefab);
        }

        private void SpawnCrystal()
        {
            _crystalSpawner.OnSpawn(this);
        }

        private void Die()
        {
            ExperiencePoints.OnKill(this);
            gameObject.SetActive(false);
        }
    }
}

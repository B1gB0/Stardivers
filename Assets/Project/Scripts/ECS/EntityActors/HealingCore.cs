using Project.Scripts.Crystals;
using Project.Scripts.Experience;
using Project.Scripts.Services;
using UnityEngine;

namespace Project.Scripts.ECS.EntityActors
{
    public class HealingCore : ResourceActor, IAcceptable
    {
        private const float CrystalJumpForce = 2.5f;
        private const float MinAngle = 0f;
        private const float MaxAngle = 360f;
        
        [SerializeField] private RedCrystal redCrystalPrefab;
        [SerializeField] private Transform _crystalSpawnPoint;

        private Vector3 _rotationCrystal;
        private Vector3 _jumpDirectionCrystal;
        private IFloatingTextService _floatingTextService;

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

        public void GetServices(IFloatingTextService floatingTextService)
        {
            _floatingTextService = floatingTextService;
        }
        
        public void AcceptScore(IScoreActorVisitor visitor)
        {
            visitor.Visit(this);
        }

        private void SpawnCrystal()
        {
            _rotationCrystal = new Vector3(MinAngle, Random.Range(MinAngle, MaxAngle), MinAngle);
            _jumpDirectionCrystal = new Vector3(Random.Range(-1, 1), 1, Random.Range(-1, 1));
            
            var crystal = Instantiate(redCrystalPrefab, _crystalSpawnPoint.position, Quaternion.Euler(_rotationCrystal));
            crystal.GetTextService(_floatingTextService);
            crystal.Rigidbody.AddForceAtPosition(_jumpDirectionCrystal * CrystalJumpForce, _crystalSpawnPoint.position, ForceMode.Impulse);
        }

        private void Die()
        {
            ExperiencePoints.OnKill(this);
            gameObject.SetActive(false);
        }
    }
}

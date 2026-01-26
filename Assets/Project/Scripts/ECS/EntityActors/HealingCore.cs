using Project.Scripts.Crystals;
using Project.Scripts.Experience;
using Project.Scripts.Services;
using UnityEngine;

namespace Project.Scripts.ECS.EntityActors
{
    public class HealingCore : ResourceActor
    {
        private const float CrystalJumpForce = 2.5f;
        private const float MinAngle = 0f;
        private const float MaxAngle = 360f;

        [SerializeField] private RedCrystal _redCrystalPrefab;
        [SerializeField] private Transform _crystalSpawnPoint;

        private Vector3 _rotationCrystal;
        private Vector3 _jumpDirectionCrystal;
        private IFloatingTextService _floatingTextService;
        private Transform _rootForObjects;

        private void OnEnable()
        {
            Health.Die += Die;
            Health.IsDamaged += SpawnCrystal;
            Health.IsDamaged += OnPlayParticleEffect;
        }

        private void OnDisable()
        {
            Health.Die -= Die;
            Health.IsDamaged -= SpawnCrystal;
            Health.IsDamaged -= OnPlayParticleEffect;
        }

        public void GetServices(IFloatingTextService floatingTextService, Transform rootForObjects)
        {
            _floatingTextService = floatingTextService;
            _rootForObjects = rootForObjects;
        }

        private void SpawnCrystal()
        {
            _rotationCrystal = new Vector3(MinAngle, Random.Range(MinAngle, MaxAngle), MinAngle);
            _jumpDirectionCrystal = new Vector3(Random.Range(-1, 1), 1, Random.Range(-1, 1));

            var crystal = Instantiate(_redCrystalPrefab,
                _crystalSpawnPoint.position,
                Quaternion.Euler(_rotationCrystal));

            crystal.transform.SetParent(_rootForObjects);
            crystal.GetTextService(_floatingTextService);
            crystal.GetHealthValue(Data.CrystalValue);

            crystal.Rigidbody.AddForceAtPosition(
                _jumpDirectionCrystal * CrystalJumpForce,
                _crystalSpawnPoint.position,
                ForceMode.Impulse);
        }

        private void Die()
        {
            ExperiencePoints.OnKill(this);
            gameObject.SetActive(false);
        }
    }
}
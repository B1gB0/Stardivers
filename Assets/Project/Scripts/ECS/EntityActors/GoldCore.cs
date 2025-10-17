using Project.Scripts.Crystals;
using Project.Scripts.Experience;
using Project.Scripts.Services;
using UnityEngine;

namespace Project.Scripts.ECS.EntityActors
{
    public class GoldCore : ResourceActor, IAcceptable
    {
        private const float MaxAngle = 360f;
        private const float CrystalJumpForce = 2.5f;
        private const float MinAngle = 0f;
        private const int MinDirection = -1;
        private const int MaxDirection = 1;
        
        [SerializeField] private GoldCrystal _goldCrystalPrefab;
        [SerializeField] private Transform _crystalSpawnPoint;
        
        private Vector3 _rotationCrystal;
        private Vector3 _jumpDirectionCrystal;
        
        private IFloatingTextService _floatingTextService;
        private IGoldService _goldService;

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
        
        public void GetServices(IFloatingTextService floatingTextService, IGoldService goldService)
        {
            _floatingTextService = floatingTextService;
            _goldService = goldService;
        }

        public void AcceptScore(IScoreActorVisitor visitor)
        {
            visitor.Visit(this);
        }

        private void SpawnCrystal()
        {
            _rotationCrystal = new Vector3(Random.Range(MinAngle, MaxAngle), Random.Range(MinAngle, MaxAngle),
                Random.Range(MinAngle, MaxAngle));
            _jumpDirectionCrystal = new Vector3(Random.Range(MinDirection, MaxDirection), MaxDirection,
                Random.Range(MinDirection, MaxDirection));
            
            var crystal = Instantiate(_goldCrystalPrefab, _crystalSpawnPoint.position,
                Quaternion.Euler(_rotationCrystal));
            crystal.GetTextService(_floatingTextService);
            crystal.GetGoldService(_goldService, (int)Data.CrystalValue);
            crystal.Rigidbody.AddForceAtPosition(_jumpDirectionCrystal * CrystalJumpForce,
                crystal.transform.position, ForceMode.Impulse);
        }
        
        private void Die()
        {
            ExperiencePoints.OnKill(this);
            gameObject.SetActive(false);
        }
    }
}
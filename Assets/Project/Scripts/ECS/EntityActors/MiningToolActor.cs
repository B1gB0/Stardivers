using Project.Scripts.Audio.Sounds;
using Project.Scripts.ParticleEffects.Effects;
using Project.Scripts.Services;
using UnityEngine;

namespace Project.Scripts.ECS.EntityActors
{
    public class MiningToolActor : MonoBehaviour
    {
        private const float MinValue = 0f;
        private const float CosMiningAngleFactor = 0.5f;

        private readonly Collider[] _colliderBuffer = new Collider[10];

        [SerializeField] private float _miningRange;
        [SerializeField] private float _miningAngle;
        [SerializeField] private float _damage;
        
        [SerializeField] private LayerMask _resourceLayerMask = 1;
        [SerializeField] private LayerMask _obstacleLayerMask = 1;

        [SerializeField] private Transform _detectionPoint;
        [SerializeField] private Transform _hitEffectPoint;

        private ResourceActor _resourceRef;

        private float _diggingSpeed;
        private float _lastHitTime;

        private AudioSoundsService _audioSoundsService;
        private ParticleEffectsService _particleEffectsService;

        private Vector3 _detectionPosition;
        private Vector3 _detectionForward;

        public bool IsMining { get; private set; }
        public Transform TargetResource => _resourceRef.transform;

        public void Construct(AudioSoundsService audioSoundsService, float diggingSpeed,
            ParticleEffectsService particleEffectsService)
        {
            _audioSoundsService = audioSoundsService;
            _diggingSpeed = diggingSpeed;
            _particleEffectsService = particleEffectsService;
        }

        private void FixedUpdate()
        {
            ResourceActor bestResource = FindBestResourceInCone();

            if (bestResource != null)
            {
                IsMining = true;

                if (_resourceRef != bestResource)
                {
                    if (_resourceRef != null)
                        _resourceRef.Health.SetHit(false);

                    _resourceRef = bestResource;
                }

                if (_lastHitTime <= MinValue)
                {
                    _audioSoundsService.PlaySound(SoundsType.Stone);
                    _resourceRef.Health.TakeDamage(_damage);
                    _resourceRef.Health.SetHit(true);

                    _particleEffectsService.PlayEffect(ParticleEffectType.MiningToolStoneHitEffect,
                        _hitEffectPoint.position);

                    _lastHitTime = _diggingSpeed;
                }

                _lastHitTime -= Time.fixedDeltaTime;
            }
            else
            {
                if (_resourceRef != null)
                    _resourceRef.Health.SetHit(false);

                _resourceRef = null;
                IsMining = false;
            }
        }

        public void ChangeDiggingSpeed(float newDiggingSpeed)
        {
            _diggingSpeed = newDiggingSpeed;
        }

        private ResourceActor FindBestResourceInCone()
        {
            _detectionPosition = _detectionPoint.position;
            _detectionForward = _detectionPoint.forward;

            if (Physics.Raycast(_detectionPosition, _detectionForward, out var hit,
                    _miningRange, _resourceLayerMask))
            {
                if (hit.collider.TryGetComponent(out ResourceActor resource))
                {
                    return resource;
                }
            }

            return FindResourceInAngle();
        }

        private ResourceActor FindResourceInAngle()
        {
            ResourceActor closestResource = null;
            float closestDistanceSqr = float.MaxValue;
            float cosHalfAngle = Mathf.Cos(_miningAngle * CosMiningAngleFactor * Mathf.Deg2Rad);

            int numColliders = Physics.OverlapSphereNonAlloc(
                _detectionPosition,
                _miningRange,
                _colliderBuffer,
                _resourceLayerMask
            );

            for (int i = 0; i < numColliders; i++)
            {
                Collider collider = _colliderBuffer[i];
                if (!collider.TryGetComponent(out ResourceActor resource))
                    continue;

                Vector3 closestPoint = collider.ClosestPoint(_detectionPosition);
                Vector3 directionToResource = closestPoint - _detectionPosition;
                float distanceSqr = directionToResource.sqrMagnitude;

                if (distanceSqr > _miningRange * _miningRange)
                    continue;

                Vector3 directionNormalized = directionToResource.normalized;
                float dotProduct = Vector3.Dot(_detectionForward, directionNormalized);

                if (dotProduct < cosHalfAngle)
                    continue;

                if (distanceSqr < closestDistanceSqr)
                {
                    closestDistanceSqr = distanceSqr;
                    closestResource = resource;
                }
            }

            return closestResource;
        }

#if UNITY_EDITOR
          private void OnDrawGizmosSelected()
        {
            if (_detectionPoint == null) return;
            
            Gizmos.color = Color.yellow;
            DrawConeGizmo3D();
            
            Gizmos.color = Color.red;
            Gizmos.DrawRay(_detectionPoint.position, _detectionPoint.forward * _miningRange);
            
            if (Application.isPlaying)
            {
                _detectionPosition = _detectionPoint.position;
                _detectionForward = _detectionPoint.forward;
                
                int numColliders = Physics.OverlapSphereNonAlloc(
                    _detectionPosition, _miningRange, _colliderBuffer, _resourceLayerMask);

                float cosHalfAngle = Mathf.Cos(_miningAngle * 0.5f * Mathf.Deg2Rad);
                
                for (int i = 0; i < numColliders; i++)
                {
                    Collider collider = _colliderBuffer[i];
                    if (collider.TryGetComponent(out ResourceActor resource))
                    {
                        Vector3 directionToResource = collider.transform.position - _detectionPosition;
                        Vector3 directionNormalized = directionToResource.normalized;
                        float dotProduct = Vector3.Dot(_detectionForward, directionNormalized);
                        
                        bool inAngle = dotProduct >= cosHalfAngle;

                        if (inAngle)
                        {
                            Gizmos.color = Color.green;
                            Gizmos.DrawLine(_detectionPosition, collider.transform.position);
                            Gizmos.DrawWireSphere(collider.transform.position, 0.3f);
                            
                            Gizmos.color = Color.blue;
                            Gizmos.DrawLine(_detectionPosition, _detectionPosition + directionNormalized * 2f);
                        }
                        else
                        {
                            Gizmos.color = inAngle ? Color.yellow : Color.gray;
                            Gizmos.DrawLine(_detectionPosition, collider.transform.position);
                        }
                    }
                }
            }
        }

        private void DrawConeGizmo3D()
        {
            int segments = 16;
            float halfAngle = _miningAngle * 0.5f * Mathf.Deg2Rad;
            
            Vector3 forward = _detectionPoint.forward * _miningRange;
            
            Vector3 lastPoint = _detectionPoint.position + Quaternion.AngleAxis(-_miningAngle * 0.5f, _detectionPoint.up) * forward;
            
            for (int i = 0; i <= segments; i++)
            {
                float angle = Mathf.Lerp(-_miningAngle * 0.5f, _miningAngle * 0.5f, (float)i / segments);
                Vector3 dir = Quaternion.AngleAxis(angle, _detectionPoint.up) * _detectionPoint.forward * _miningRange;
                Vector3 point = _detectionPoint.position + dir;
                
                Gizmos.DrawLine(_detectionPoint.position, point);
                
                if (i > 0)
                {
                    Gizmos.DrawLine(lastPoint, point);
                }
                
                lastPoint = point;
            }
            
            for (int i = 0; i <= 4; i++)
            {
                float verticalAngle = Mathf.Lerp(-30f, 30f, (float)i / 4);
                Vector3 verticalDir = Quaternion.AngleAxis(verticalAngle, _detectionPoint.right) * _detectionPoint.forward * _miningRange;
                Gizmos.DrawLine(_detectionPoint.position, _detectionPoint.position + verticalDir);
            }
        }
#endif
    }
}
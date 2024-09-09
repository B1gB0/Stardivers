using System;
using Build.Game.Scripts.ECS.Components;
using Project.Game.Scripts;
using UnityEngine;

namespace Build.Game.Scripts.ECS.EntityActors
{
    public class MiningToolActor : MonoBehaviour
    {
        private const string Stone = nameof(Stone);
        private const float MinValue = 0f;

        [SerializeField] private float _miningRange;
        [SerializeField] private float _delay;
        [SerializeField] private float _damage;
        
        [SerializeField] private Transform _detectionPoint;
        [SerializeField] private Transform _hitEffectPoint;
        [SerializeField] private ParticleSystem _hitEffect;

        private ParticleSystem _hitEffectRef;
        private StoneActor stoneRef;
        private float _lastHitTime;
        private AudioSoundsService _audioSoundsService;

        public bool IsMining { get; private set; }

        private void Awake()
        {
            _hitEffectRef = Instantiate(_hitEffect, _hitEffectPoint);
            _hitEffectRef.Stop();
        }

        private void Update()
        {
            if (Physics.Raycast(_detectionPoint.position, _detectionPoint.forward, out var hit,
                    _miningRange))
            {
                if(stoneRef != null)
                    stoneRef.Health.SetHit(false);
                
                if (!hit.collider.TryGetComponent(out StoneActor resource)) return;
                
                IsMining = true;

                if (_lastHitTime <= MinValue)
                {
                    stoneRef = resource;

                    _audioSoundsService.PlaySound(Stone);

                    stoneRef.Health.TakeDamage(_damage);
                    stoneRef.Health.SetHit(true);

                    _hitEffectRef.Play();

                    _lastHitTime = _delay;
                }

                _lastHitTime -= Time.deltaTime;
            }
            else
            {
                IsMining = false;
            }
        }

        public void GetAudioService(AudioSoundsService audioSoundsService)
        {
            _audioSoundsService = audioSoundsService;
        }
    }
}
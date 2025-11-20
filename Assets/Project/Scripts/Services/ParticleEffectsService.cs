using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Project.Scripts.ParticleEffects;
using Project.Scripts.ParticleEffects.Effects;
using Reflex.Attributes;
using UnityEngine;

namespace Project.Scripts.Services
{
    public class ParticleEffectsService : MonoBehaviour, IService
    {
        private const string PlayerHitEffectPath = "PlayerHitEffect";
        private const string EnemyHitEffectPath = "EnemyHitEffect";
        private const string BigEnemyHitEffectPath = "BigEnemyHitEffect";
        private const string StoneHitEffectPath = "StoneHitEffect";
        private const string MineExplosionEffectPath = "MineExplosionEffect";
        private const string FragGrenadeExplosionEffectPath = "FragGrenadeExplosionEffect";
        private const string IceCrystalExplosionEffectPath = "IceCrystalExplosionEffect";
        private const string CapsuleExplosionEffectPath = "CapsuleExplosionEffect";
        private const string MiningToolStoneHitEffectPath = "MiningToolStoneHitEffect";
        
        private const string ParticleEffects = nameof(ParticleEffects);

        private Dictionary<ParticleEffectType, ParticleEffect> _effectDictionary;
        private Dictionary<ParticleEffectType, Queue<ParticleSystem>> _particlePool;
        private IResourceService _resourceService;
        private Transform _particleParent;

        public bool IsInitiated { get; private set; }

        [Inject]
        private void Construct(IResourceService resourceService)
        {
            _resourceService = resourceService;
        }

        public async UniTask Init()
        {
            if (IsInitiated)
                return;
            
            _particleParent = new GameObject(ParticleEffects).transform;
            _particleParent.SetParent(transform);
            
            _particlePool = new Dictionary<ParticleEffectType, Queue<ParticleSystem>>();

            await InitializeEffectDictionary();

            IsInitiated = true;
        }

        public void PlayEffect(ParticleEffectType effectType, Vector3 position)
        {
            if (!IsInitiated) return;
            if (!_effectDictionary.ContainsKey(effectType)) return;

            PlayEffectAsync(effectType, position).Forget();
        }

        private async UniTask InitializeEffectDictionary()
        {
            var builder = new ParticleEffectBuilder(_resourceService)
                .AddScriptableObject(ParticleEffectType.PlayerHit, PlayerHitEffectPath)
                .AddScriptableObject(ParticleEffectType.EnemyHit, EnemyHitEffectPath)
                .AddScriptableObject(ParticleEffectType.BigEnemyHit, BigEnemyHitEffectPath)
                .AddScriptableObject(ParticleEffectType.StoneHit, StoneHitEffectPath)
                .AddScriptableObject(ParticleEffectType.MineExplosion, MineExplosionEffectPath)
                .AddScriptableObject(ParticleEffectType.FragGrenadeExplosion, FragGrenadeExplosionEffectPath)
                .AddScriptableObject(ParticleEffectType.IceCrystalExplosion, IceCrystalExplosionEffectPath)
                .AddScriptableObject(ParticleEffectType.CapsulePartsExplosion, CapsuleExplosionEffectPath)
                .AddScriptableObject(ParticleEffectType.MiningToolStoneHitEffect, MiningToolStoneHitEffectPath);

            _effectDictionary = await builder.Build();
        }

        private async UniTaskVoid PlayEffectAsync(ParticleEffectType effectType, Vector3 position)
        {
            var particleEffect = GetOrCreateParticleSystem(effectType);

            var transformOfEffect = particleEffect.transform;
            transformOfEffect.position = position;

            particleEffect.Play(true);
            
            await WaitForParticleSystem(particleEffect);
            
            ReturnParticleSystemToPool(effectType, particleEffect);
        }
        
        private ParticleSystem GetOrCreateParticleSystem(ParticleEffectType effectType)
        {
            if (!_particlePool.ContainsKey(effectType))
            {
                _particlePool[effectType] = new Queue<ParticleSystem>();
            }

            var pool = _particlePool[effectType];
            
            while (pool.Count > 0)
            {
                var particleEffect = pool.Dequeue();
                
                if (particleEffect == null || particleEffect.isPlaying)
                    continue;
                
                particleEffect.gameObject.SetActive(true);
                return particleEffect;
            }
            
            return CreateNewParticleSystem(effectType);
        }

        private ParticleSystem CreateNewParticleSystem(ParticleEffectType effectType)
        {
            if (!_effectDictionary.ContainsKey(effectType)) return null;

            var config = _effectDictionary[effectType];
            var particleEffect = Instantiate(config.Effect, _particleParent);
            
            return particleEffect;
        }

        private void ReturnParticleSystemToPool(ParticleEffectType effectType, ParticleSystem particleSystem)
        {
            if (particleSystem == null) return;
            
            particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            particleSystem.gameObject.SetActive(false);
            particleSystem.Clear(true);
            
            if (_particlePool.TryGetValue(effectType, out var queueEffects))
            {
                queueEffects.Enqueue(particleSystem);
            }
        }

        private async UniTask WaitForParticleSystem(ParticleSystem particleSystem)
        {
            if (particleSystem == null) return;
            
            await UniTask.WaitUntil(() => particleSystem == null || !particleSystem.IsAlive(true));
        }
        
        private void OnDestroy()
        {
            if (_particlePool == null)
                return;
            
            foreach (var pool in _particlePool.Values)
            {
                foreach (var particleSystem in pool)
                {
                    if (particleSystem != null)
                        Destroy(particleSystem.gameObject);
                }
            }
            _particlePool.Clear();
        }
    }
}
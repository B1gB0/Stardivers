using UnityEngine;

namespace Project.Scripts.ParticleEffects.Effects
{
    public abstract class ParticleEffect : ScriptableObject
    {
        [field: SerializeField] public ParticleSystem Effect { get; private set; }
    }
}
using UnityEngine;

namespace Project.Scripts.Audio.Sounds
{
    public abstract class Sound : ScriptableObject
    {
        [field: SerializeField] public AudioClip Clip { get; private set; }
        [field: SerializeField] public bool IsLoop { get; private set; }
        [field: SerializeField] public float Volume { get; private set; } = 1f;
    }
}

using System;
using UnityEngine;

namespace Project.Scripts.Audio.Sounds
{
    [Serializable]
    public class Sound
    {
        public string ClipName;
        public AudioClip Clip;
        public bool IsLoop;
        
        [Range(0f, 1f)] public float Volume = 1f;
    }
}
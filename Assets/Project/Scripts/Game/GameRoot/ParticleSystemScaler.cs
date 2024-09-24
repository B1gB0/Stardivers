using UnityEngine;

namespace Project.Scripts.Game.GameRoot
{
    public class ParticleSystemScaler : MonoBehaviour
    {
        private readonly Vector3 _portraitScale = new (1f, 2f, 1f);
        private readonly Vector3 _landscapeScale = new (1f, 1f, 1f);

        [SerializeField] private ParticleSystem _effect;

        private void Start()
        {
            _effect.transform.localScale = Screen.orientation == ScreenOrientation.Portrait ? _portraitScale : _landscapeScale;
        }
    }
}

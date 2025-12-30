using UnityEngine;

namespace Project.Scripts.Levels.Outpost
{
    [RequireComponent(typeof(Animator))]
    public class Entrance : MonoBehaviour
    {
        public readonly int IsOpened = Animator.StringToHash(nameof(IsOpened));

        private Animator _animator;

        private void Start()
        {
            _animator = GetComponent<Animator>();
        }

        public void OpenGate()
        {
            if (_animator.GetBool(IsOpened))
                return;

            _animator.SetBool(IsOpened, true);
        }

        public void CloseGate()
        {
            if (!_animator.GetBool(IsOpened))
                return;

            _animator.SetBool(IsOpened, false);
        }
    }
}
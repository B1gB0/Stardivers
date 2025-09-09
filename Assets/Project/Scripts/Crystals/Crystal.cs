using Project.Scripts.Services;
using UnityEngine;

namespace Project.Scripts.Crystals
{
    public abstract class Crystal : MonoBehaviour
    {
        protected IFloatingTextService TextService;

        public Rigidbody Rigidbody { get; private set; }

        private void Awake()
        {
            Rigidbody = GetComponent<Rigidbody>();
        }

        public void GetTextService(IFloatingTextService textService)
        {
            TextService = textService;
        }
    }
}
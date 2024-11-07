using Project.Scripts.Services;
using UnityEngine;

namespace Project.Scripts.Crystals
{
    public class Crystal : MonoBehaviour
    {
        protected FloatingTextService TextService;

        [SerializeField] protected Color ColorText;
        
        public Rigidbody Rigidbody { get; private set; }

        private void Awake()
        {
            Rigidbody = GetComponent<Rigidbody>();
        }

        public void GetTextService(FloatingTextService textService)
        {
            TextService = textService;
        }
    }
}
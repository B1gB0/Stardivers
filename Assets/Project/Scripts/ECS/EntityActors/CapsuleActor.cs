using UnityEngine;

namespace Project.Scripts.ECS.EntityActors
{
    public class CapsuleActor : MonoBehaviour
    {
        [SerializeField] private CapsuleParts _capsuleParts;

        public void Destroy()
        {
            _capsuleParts = Instantiate(_capsuleParts, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}

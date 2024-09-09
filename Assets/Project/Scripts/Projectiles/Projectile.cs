using System.Collections;
using UnityEngine;

namespace Project.Scripts.Projectiles
{
    public abstract class Projectile : MonoBehaviour
    {
        [field: SerializeField] public float LifeTime { get; private set; } = 4f;

        private void OnEnable()
        {
            StartCoroutine(LifeRoutine());
        }

        private void OnDisable()
        {
            StopCoroutine(LifeRoutine());
        }

        protected virtual IEnumerator LifeRoutine()
        {
            yield return new WaitForSeconds(LifeTime);
        
            gameObject.SetActive(false);
        }
    }
}
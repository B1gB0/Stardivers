using Project.Scripts.Projectiles;
using UnityEngine;

namespace Project.Scripts.Lightning
{
    public class LightningTrailRendererProjectile : Projectile
    {
        private Vector3 _targetPosition;

        protected override void FixedUpdate()
        {
            if(transform.position == _targetPosition)
                gameObject.SetActive(false);
            
            base.FixedUpdate();
        }

        public void SetPosition(Vector3 endPoint)
        {
            _targetPosition = endPoint;
            Direction = (endPoint - transform.position).normalized;
            Transform.forward = Direction;
        }
    }
}
using UnityEngine;

namespace Project.Scripts.Weapon.Player
{
    public class EnemyDetectorForPlayer : EnemyDetector
    {
        private Transform _target;
        
        public void Construct(Transform target)
        {
            _target = target;
        }
        
        private void Update()
        {
            transform.position = new Vector3(_target.position.x, _target.position.y, _target.position.z);
        }
    }
}
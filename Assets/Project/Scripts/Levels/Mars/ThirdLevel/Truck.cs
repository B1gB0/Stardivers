using System.Collections.Generic;
using Project.Scripts.Levels.Triggers;
using UnityEngine;

namespace Project.Scripts.Levels.Mars.ThirdLevel
{
    public class Truck : MonoBehaviour
    {
        private const int MinCountPoints = 0;
        private const float MinHeight = 0;
        private const float MinDistanceToPoint = 1f;
        private const float MoveSpeed = 2f;
        private const float RotationSpeed = 2f;

        [SerializeField] private Transform[] _followPoints;
        [SerializeField] private List<WheelRotation> _wheels;
        [SerializeField] private TruckObstacleTrigger _obstacleForwardTrigger;

        private TruckTrigger _truckTrigger;
        private int _currentIndexPoint;
        private float _heightAboveGroundLevel;

        private void Start()
        {
            _truckTrigger = GetComponentInChildren<TruckTrigger>();
            _heightAboveGroundLevel = transform.position.y;
        }

        private void FixedUpdate()
        {
            if (_truckTrigger.IsPlayerNearby && !_obstacleForwardTrigger.IsObstacleForward)
            {
                Vector3 target = _followPoints[_currentIndexPoint].position;

                MoveTowardsTarget(target);

                SmoothRotateTowardsTarget(target);

                foreach (var wheel in _wheels)
                {
                    wheel.OnRotateWheel(true);
                }

                if (Vector3.Distance(transform.position, target) < MinDistanceToPoint)
                    GoToNextPoint();
            }
            else
            {
                foreach (var wheel in _wheels)
                {
                    wheel.OnRotateWheel(false);
                }
            }
        }

        private void MoveTowardsTarget(Vector3 targetPosition)
        {
            targetPosition.y = _heightAboveGroundLevel;

            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPosition,
                MoveSpeed * Time.fixedDeltaTime
            );
        }

        private void SmoothRotateTowardsTarget(Vector3 targetPosition)
        {
            Vector3 direction = (targetPosition - transform.position).normalized;

            direction.y = MinHeight;

            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);

                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    targetRotation,
                    RotationSpeed * Time.fixedDeltaTime
                );
            }
        }

        private void GoToNextPoint()
        {
            if (_followPoints.Length == MinCountPoints) return;

            _currentIndexPoint++;
        }

        void OnDrawGizmos()
        {
            if (_followPoints == null || _followPoints.Length < 2) return;

            // Рисуем линии между точками пути
            Gizmos.color = Color.blue;
            for (int i = 0; i < _followPoints.Length - 1; i++)
            {
                Gizmos.DrawLine(_followPoints[i].position, _followPoints[i + 1].position);
            }

            // Рисуем сферы в точках пути
            Gizmos.color = Color.red;
            foreach (Transform point in _followPoints)
            {
                Gizmos.DrawSphere(point.position, 0.2f);
            }
        }
    }
}
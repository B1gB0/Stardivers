using System.Collections.Generic;
using Project.Scripts.Levels.Triggers;
using UnityEngine;

namespace Project.Scripts.Levels.Mars.ThirdLevel
{
    [RequireComponent(typeof(Rigidbody))]
    public class Truck : MonoBehaviour
    {
        private const int MinCountPoints = 0;
        private const int NextPoint = 1;
        
        private const float MinHeight = 0f;
        private const float MinDistanceToPoint = 1f;
        private const float MoveSpeed = 2f;
        private const float RotationSpeed = 2f;

        [SerializeField] private Transform[] _followPoints;
        [SerializeField] private List<WheelRotation> _wheels;
        [SerializeField] private TruckObstacleTrigger _obstacleForwardTrigger;

        private TruckPlayerTrigger _truckPlayerTrigger;
        private int _currentIndexPoint;
        private float _heightAboveGroundLevel;
        private bool _isFinalPointReached;

        private void Start()
        {
            _truckPlayerTrigger = GetComponentInChildren<TruckPlayerTrigger>();
            _heightAboveGroundLevel = transform.position.y;
        }

        private void FixedUpdate()
        {
            if (_truckPlayerTrigger.IsPlayerNearby && !_obstacleForwardTrigger.IsObstacleForward && !_isFinalPointReached)
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

        public void ReachFinalPoint()
        {
            _isFinalPointReached = true;
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

            var nextIndex = _currentIndexPoint + NextPoint;

            if (nextIndex > _followPoints.Length - 1)
            {
                _isFinalPointReached = true;
            }
            else
            {
                _currentIndexPoint = nextIndex;
            }
        }
    }
}
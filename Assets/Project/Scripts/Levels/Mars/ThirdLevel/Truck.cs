using System.Collections.Generic;
using Project.Scripts.Levels.Triggers;
using UnityEngine;
using UnityEngine.AI;

namespace Project.Scripts.Levels.Mars.ThirdLevel
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class Truck : MonoBehaviour
    {
        private const int MinCountPoints = 0;
        private const float MinDistanceToPoint = 0.5f;
        
        [SerializeField] private Transform[] _followPoints;
        [SerializeField] private List<WheelRotation> _wheels;

        private TruckTrigger _truckTrigger;
        private NavMeshAgent _navMeshAgent;
        private int _destPoint;

        private void Start()
        {
            _truckTrigger = GetComponentInChildren<TruckTrigger>();
            _navMeshAgent = GetComponent<NavMeshAgent>();

            _navMeshAgent.autoBraking = false;
        }

        private void FixedUpdate()
        {
            if (_truckTrigger.IsPlayerNearby)
            {
                _navMeshAgent.isStopped = false;
                
                foreach (var wheel in _wheels)
                {
                    wheel.OnRotateWheel(true);
                }

                if (!_navMeshAgent.pathPending && _navMeshAgent.remainingDistance < MinDistanceToPoint)
                    GoToNextPoint();
            }
            else
            {
                _navMeshAgent.isStopped = true;
                
                foreach (var wheel in _wheels)
                {
                    wheel.OnRotateWheel(false);
                }
            }
        }

        private void GoToNextPoint()
        {
            if(_followPoints.Length == MinCountPoints) return;
            
            _navMeshAgent.destination = _followPoints[_destPoint].position;
            _destPoint = (_destPoint + 1) % _followPoints.Length;
        }
    }
}
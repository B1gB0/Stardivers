using System.Collections;
using Project.Scripts.ECS.System;
using UnityEngine;

namespace Project.Scripts.Levels.Spawners
{
    public class PlayerSpawner : MonoBehaviour
    {
        private const int MinValue = 0;
        private const float DefaultCapsuleMoveSpeed = 5f;
        
        private readonly GameInitSystem _gameInitSystem;
        private readonly Transform _playerTransform;
        
        private Transform _capsuleTransform;

        public void SpawnPlayer()
        {
            _gameInitSystem.Player.gameObject.SetActive(true);

            if (_gameInitSystem.Player.Health.TargetHealth <= MinValue)
            {
                _gameInitSystem.Player.Health.SetHealthValue();
            }
        }

        public void SpawnCapsule()
        {
            _gameInitSystem.CreateCapsule();
            _capsuleTransform = _gameInitSystem.Capsule.transform;
            StartCoroutine(LaunchPlayerCapsule());
        }

        private IEnumerator LaunchPlayerCapsule()
        {
            _capsuleTransform.position = Vector3.MoveTowards(
                _capsuleTransform.position, -_playerTransform.position, 
                DefaultCapsuleMoveSpeed * Time.deltaTime);

            if (_capsuleTransform.position == _playerTransform.position)
            {
                SpawnPlayer();
                _gameInitSystem.Capsule.Destroy();
            }

            yield return null;
        }
    }
}
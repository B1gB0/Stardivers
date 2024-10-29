using System.Collections.Generic;
using Project.Game.Scripts.Player.PlayerInputModule;
using UnityEngine;

namespace Build.Game.Scripts.ECS.EntityActors
{
    public class PlayerActor : MonoBehaviour
    {
        [field: SerializeField] public Health Health { get; private set; }
        
        [field: SerializeField] public Animator Animator { get; private set; }

        [field: SerializeField] public Rigidbody Rigidbody { get; private set; }

        [field: SerializeField] public PlayerInputController PlayerInputController { get; private set; }
        
        [field: SerializeField] public MiningToolActor MiningToolActor { get; private set; }

        [field: SerializeField] public List<Weapon> Weapons { get; private set; }

        private void OnEnable()
        {
            Health.Die += Die;
        }

        private void OnDisable()
        {
            Health.Die -= Die;
        }

        private void Die()
        {
            gameObject.SetActive(false);
        }
    }
}
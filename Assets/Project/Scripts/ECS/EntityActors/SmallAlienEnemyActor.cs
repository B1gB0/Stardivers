using System;
using Project.Scripts.Score;
using Project.Scripts.UI;
using UnityEngine;
using UnityEngine.AI;

namespace Build.Game.Scripts.ECS.EntityActors
{
    public class SmallAlienEnemyActor : ScoreActor
    {
        [field: SerializeField] public Health Health{ get; private set; }
        
        [field: SerializeField] public Animator Animator { get; private set; }

        [field: SerializeField] public Rigidbody Rigidbody { get; private set; }
        
        [field: SerializeField] public NavMeshAgent NavMeshAgent { get; private set; }

        private ExperiencePoints _experiencePoints;
        private FloatingDamageTextService _damageTextService;

        public void Construct(ExperiencePoints experiencePoints, FloatingDamageTextService damageTextService)
        {
            _experiencePoints = experiencePoints;
            
            _damageTextService = damageTextService;
            Health.IsDamaged += _damageTextService.OnChangedDamageText;
        }

        private void OnEnable()
        {
            Health.Die += Die;
        }

        private void OnDisable()
        {
            Health.Die -= Die;
        }
        
        public override void Accept(IActorVisitor visitor)
        {
            visitor.Visit(this);
        }

        private void Die()
        {
            Health.IsDamaged -= _damageTextService.OnChangedDamageText;
            _experiencePoints.OnKill(this);
            gameObject.SetActive(false);
        }
    }
}
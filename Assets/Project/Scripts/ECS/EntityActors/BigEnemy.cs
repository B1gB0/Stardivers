using Leopotam.Ecs;
using Project.Scripts.ECS.Components;
using Project.Scripts.Experience;
using Project.Scripts.Weapon.Enemy;
using UnityEngine;
using UnityEngine.AI;

namespace Project.Scripts.ECS.EntityActors
{
    public class BigEnemy : EnemyActor, IAcceptable
    {
        [field: SerializeField] public BigEnemyAlienWeapon Weapon { get; private set; }
        [field: SerializeField] public NavMeshAgent NavMeshAgent { get; private set; }

        private void OnEnable()
        {
            Health.Die += OnDie;
        }

        private void OnDisable()
        {
            Health.Die -= OnDie;
        }
        
        public void AcceptScore(IScoreActorVisitor visitor)
        {
            visitor.Visit(this);
        }
        
        public void ChangeMoveSpeed(float moveSpeed)
        {
            ref var movableComponent = ref EnemyEntity.Get<EnemyMovableComponent>();
            movableComponent.MoveSpeed = moveSpeed;
            movableComponent.NavMeshAgent.speed = moveSpeed;
        }

        protected override void OnDie()
        {
            Health.IsSpawnedDamageText -= TextService.OnChangedFloatingText;
            ExperiencePoints.OnKill(this);
            base.OnDie();
            
            gameObject.SetActive(false);
        }
    }
}
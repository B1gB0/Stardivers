using Leopotam.Ecs;
using Project.Scripts.ECS.Components;

namespace Project.Scripts.ECS.System
{
    public class AttackCheckSystem : IEcsRunSystem
    {
        private readonly EcsFilter<FollowPlayerComponent, EnemyMovableComponent> _enemyFilter;

        public void Run()
        {
            foreach (var entity in _enemyFilter)
            {
                ref var followComponent = ref _enemyFilter.Get1(entity);
                ref var movableComponent = ref _enemyFilter.Get2(entity);

                if (followComponent.Target == null || !movableComponent.NavMeshAgent.isActiveAndEnabled
                                                   || !followComponent.Target.CanFollow)
                {
                    movableComponent.IsAttack = false;
                    continue;
                }

                movableComponent.IsAttack = movableComponent.NavMeshAgent.remainingDistance <=
                                            movableComponent.NavMeshAgent.stoppingDistance;
            }
        }
    }
}
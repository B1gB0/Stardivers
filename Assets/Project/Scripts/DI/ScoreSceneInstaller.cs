using Build.Game.Scripts.ECS.EntityActors;
using DI;
using Project.Scripts.Score;
using UnityEngine;
using Zenject;

public class ScoreSceneInstaller : MonoInstaller
{
    [SerializeField] private EnemyActor _enemyActor;
    [SerializeField] private StoneActor _stoneActor;
    
    [SerializeField] private ExperiencePoints experiencePoints;
    
    public override void InstallBindings()
    {
        Container.BindFactory<EnemyActor, EnemyFactory>().FromComponentInNewPrefab(_enemyActor);
        Container.BindFactory<StoneActor, StoneFactory>().FromComponentInNewPrefab(_stoneActor);
        
        Container.Bind<ExperiencePoints>().FromInstance(experiencePoints).AsSingle();
    }
}

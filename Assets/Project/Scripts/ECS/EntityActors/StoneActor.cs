using System;
using Project.Scripts.Score;
using UnityEngine;
using Zenject;

namespace Build.Game.Scripts.ECS.EntityActors
{
    public class StoneActor : ScoreActor
    {
        [field: SerializeField] public Health Health{ get; private set; }

        [field: SerializeField] public Animator Animator { get; private set; }

        private ExperiencePoints experiencePoints;

        public void Construct(ExperiencePoints experiencePoints)
        {
            this.experiencePoints = experiencePoints;
        }

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
            experiencePoints.OnKill(this);
            gameObject.SetActive(false);
        }

        public override void Accept(IActorVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
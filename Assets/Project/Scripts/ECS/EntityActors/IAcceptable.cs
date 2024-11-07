using Project.Scripts.Crystals;
using Project.Scripts.Experience;
using UnityEngine;

namespace Project.Scripts.ECS.EntityActors
{
    public interface IAcceptable
    {
        void AcceptScore(IScoreActorVisitor visitor) { }
    }
}

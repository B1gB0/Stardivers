using Project.Scripts.Crystals;
using Project.Scripts.Experience;
using Project.Scripts.MiningResources;
using UnityEngine;

namespace Project.Scripts.ECS.EntityActors
{
    public interface IAcceptable
    {
        void AcceptScore(IScoreActorVisitor visitor) { }
         
        void AcceptSpawnCrystal(ICrystalsActorVisitor visitor) { }
    }
}

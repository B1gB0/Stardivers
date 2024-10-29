using Project.Scripts.ECS.EntityActors;
using Project.Scripts.MiningResources;

namespace Project.Scripts.Crystals
{
    public interface ICrystalsActorVisitor
    {
        void Visit(HealingCore healingCore, HealingCrystal healingCrystal);
    
        void Visit(GoldCore goldCore, GoldCrystal goldCrystalPrefab);
    }
}
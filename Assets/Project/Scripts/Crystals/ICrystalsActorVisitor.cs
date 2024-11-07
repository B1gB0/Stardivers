using Project.Scripts.ECS.EntityActors;

namespace Project.Scripts.Crystals
{
    public interface ICrystalsActorVisitor
    {
        void Visit(HealingCrystal healingCrystal);
    
        void Visit(GoldCrystal goldCrystal);
    }
}
using Project.Scripts.ECS.EntityActors;

namespace Project.Scripts.Crystals
{
    public interface ICrystalsActorVisitor
    {
        void Visit(RedCrystal redCrystal);
    
        void Visit(GoldCrystal goldCrystal);
    }
}
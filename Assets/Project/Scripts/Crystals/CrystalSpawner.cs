using Project.Scripts.ECS.EntityActors;

namespace Project.Scripts.Crystals
{
    public class CrystalSpawner
    {
        private readonly CrystalsActorVisitor crystalsActorVisitor = new ();
        
        public void OnSpawn(IAcceptable resource)
        {
            resource.AcceptSpawnCrystal(crystalsActorVisitor);
        }
    }
}
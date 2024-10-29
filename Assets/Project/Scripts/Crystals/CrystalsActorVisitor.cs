using Project.Scripts.ECS.EntityActors;
using Project.Scripts.MiningResources;
using UnityEngine;

namespace Project.Scripts.Crystals
{
    public class CrystalsActorVisitor : MonoBehaviour, ICrystalsActorVisitor
    {
        public void Visit(HealingCore healingCore, HealingCrystal healingCrystalPrefab)
        {
            Instantiate(healingCrystalPrefab, healingCore.transform.position, Quaternion.identity);
        }

        public void Visit(GoldCore goldCore, GoldCrystal goldCrystalPrefab)
        {
            Instantiate(goldCrystalPrefab, goldCore.transform.position, Quaternion.identity);
        }
    }
}
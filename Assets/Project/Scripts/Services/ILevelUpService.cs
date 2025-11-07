using System.Collections.Generic;
using Project.Scripts.Cards;
using Project.Scripts.UI.View;
using Project.Scripts.Weapon.Player;

namespace Project.Scripts.Services
{
    public interface ILevelUpService : IService
    {
        public void RemoveImprovementCard(ImprovementCard improvementCard);
        public void RemoveWeaponCard(WeaponType type);
        public void GenerateCardsByLevel(int currentLevel, WeaponHolder weaponHolder, List<CardView> cardViews);
        public void GenerateImprovements(List<CardView> cardViews);
        public void RecreateCards();
        public void UpdateImprovementCardsByTypeWeapon(WeaponType type);
    }
}
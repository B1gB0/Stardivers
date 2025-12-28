using System.Collections.Generic;
using Project.Scripts.Cards;

namespace Project.Scripts.Services
{
    public interface ICardService : IService
    {
        public List<ImprovementCard> ImprovementCards { get; }
        public List<WeaponCard> WeaponCards { get; }
    }
}
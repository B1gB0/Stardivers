using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Project.Scripts.Cards;

namespace Project.Scripts.Services
{
    public interface ICardService
    {
        public List<ImprovementCard> ImprovementCards { get; }
        public List<WeaponCard> WeaponCards { get; }
        public UniTask Init();
    }
}
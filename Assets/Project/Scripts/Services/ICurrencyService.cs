using System;
using Cysharp.Threading.Tasks;

namespace Project.Scripts.Services
{
    public interface ICurrencyService
    {
        public event Action<int> OnGoldValueChanged;
        public event Action<int> OnAlienCocoonValueChanged;
        
        public int Gold { get; }
        public int AlienCocoons { get; }
        public int MaxAlienCocoons { get; }

        public UniTask Init();
        public void SetGold(int gold);
        public void AddGold(int gold);
        public void AddAlienCocoon(int alienCocoon);
        public void SpendGold(int gold);
        public void SetMaxAlienCocoons(int maxAlienCocoons);
    }
}
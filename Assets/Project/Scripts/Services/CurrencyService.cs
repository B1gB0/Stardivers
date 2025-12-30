using System;
using Cysharp.Threading.Tasks;
using YG;

namespace Project.Scripts.Services
{
    public class CurrencyService : ICurrencyService
    {
        private const int MinValue = 0;

        public event Action<int> OnGoldValueChanged;
        public event Action<int> OnAlienCocoonValueChanged;
        public event Action OnAllAlienCocoonsCollected;

        public int Gold { get; private set; }
        public int AccumulatedGold { get; private set; }
        public int AlienCocoons { get; private set; }
        public int MaxAlienCocoons { get; private set; }
        public bool IsInitiated { get; private set; }

        public UniTask Init()
        {
            if (IsInitiated)
                return UniTask.CompletedTask;

            Gold = YG2.saves.Gold;
            OnGoldValueChanged?.Invoke(Gold);

            IsInitiated = true;

            return UniTask.CompletedTask;
        }

        public void SetGold(int gold)
        {
            Gold = gold;
            OnGoldValueChanged?.Invoke(Gold);
            SaveGold();
        }

        public void AddGold(int gold)
        {
            Gold += gold;
            AccumulatedGold += gold;
            OnGoldValueChanged?.Invoke(Gold);
            SaveGold();
        }

        public void SpendGold(int gold)
        {
            Gold -= gold;
            OnGoldValueChanged?.Invoke(Gold);
            SaveGold();
        }

        public void ResetAccumulatedGold()
        {
            AccumulatedGold = MinValue;
        }

        public void AddAlienCocoon(int alienCocoon)
        {
            AlienCocoons += alienCocoon;
            OnAlienCocoonValueChanged?.Invoke(AlienCocoons);

            if (AlienCocoons == MaxAlienCocoons)
                OnAllAlienCocoonsCollected?.Invoke();
        }

        public void SetMaxAlienCocoons(int maxAlienCocoons)
        {
            MaxAlienCocoons = maxAlienCocoons;
        }

        private void SaveGold()
        {
            YG2.saves.Gold = Gold;
            YG2.SaveProgress();
        }
    }
}
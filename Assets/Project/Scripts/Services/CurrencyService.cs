using System;
using Cysharp.Threading.Tasks;
using YG;

namespace Project.Scripts.Services
{
    public class CurrencyService : Service, ICurrencyService
    {
        public event Action<int> OnGoldValueChanged;
        public event Action<int> OnAlienCocoonValueChanged;
    
        public int Gold { get; private set; }
        public int AlienCocoons { get; private set; }
        public int MaxAlienCocoons { get; private set; }

        public override UniTask Init()
        {
            Gold = YG2.saves.Gold;
            OnGoldValueChanged?.Invoke(Gold);

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
            OnGoldValueChanged?.Invoke(Gold);
            SaveGold();
        }

        public void AddAlienCocoon(int alienCocoon)
        {
            AlienCocoons += alienCocoon;
            OnAlienCocoonValueChanged?.Invoke(AlienCocoons);
        }

        public void SpendGold(int gold)
        {
            Gold -= gold;
            OnGoldValueChanged?.Invoke(Gold);
            SaveGold();
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

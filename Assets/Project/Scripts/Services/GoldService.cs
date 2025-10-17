using System;
using Cysharp.Threading.Tasks;
using YG;

namespace Project.Scripts.Services
{
    public class GoldService : Service, IGoldService
    {
        public event Action<int> OnValueChanged;
    
        public int Gold { get; private set; }

        public override UniTask Init()
        {
            Gold = YG2.saves.Gold;
            OnValueChanged?.Invoke(Gold);

            return UniTask.CompletedTask;
        }

        public void SetGold(int gold)
        {
            Gold = gold;
            OnValueChanged?.Invoke(Gold);
            SaveGold();
        }

        public void AddGold(int gold)
        {
            Gold += gold;
            OnValueChanged?.Invoke(Gold);
            SaveGold();
        }

        public void SpendGold(int gold)
        {
            Gold -= gold;
            OnValueChanged?.Invoke(Gold);
            SaveGold();
        }

        private void SaveGold()
        {
            YG2.saves.Gold = Gold;
            YG2.SaveProgress();
        }
    }
}

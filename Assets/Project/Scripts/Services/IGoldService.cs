using System;
using Cysharp.Threading.Tasks;

namespace Project.Scripts.Services
{
    public interface IGoldService
    {
        public event Action<int> OnValueChanged;
        
        public int Gold { get; }

        public UniTask Init();
        public void SetGold(int gold);
        public void AddGold(int gold);
        public void SpendGold(int gold);
    }
}
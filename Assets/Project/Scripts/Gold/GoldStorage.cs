using System;
using UnityEngine;

namespace Project.Scripts.Gold
{
    public class GoldStorage : MonoBehaviour
    {
        public event Action<int> OnValueChanged;
    
        public int Gold { get; private set; }

        public void SetGold(int gold)
        {
            Gold = gold;
            OnValueChanged?.Invoke(Gold);
        }

        public void AddGold(int gold)
        {
            Gold += gold;
            OnValueChanged?.Invoke(Gold);
        }

        public void SpendGold(int gold)
        {
            Gold -= gold;
            OnValueChanged?.Invoke(Gold);
        }
    }
}

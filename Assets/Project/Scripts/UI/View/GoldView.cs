using System;
using TMPro;
using UnityEngine;
using YG;

namespace Project.Scripts.UI.View
{
    public class GoldView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;

        public int AccumulatedGold { get; private set; }

        private void Start()
        {
            AccumulatedGold = YG2.saves.gold;
            _text.text = AccumulatedGold.ToString();
        }

        public void SetValue(int value)
        {
            AccumulatedGold += value;
            _text.text = AccumulatedGold.ToString();
            YG2.saves.gold = AccumulatedGold;
            YG2.SaveProgress();
        }
    }
}

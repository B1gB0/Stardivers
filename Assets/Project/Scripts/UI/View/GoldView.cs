using TMPro;
using UnityEngine;

namespace Project.Scripts.UI.View
{
    public class GoldView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;

        public int AccumulatedGold { get; private set; }

        public void SetValue(int value)
        {
            AccumulatedGold += value;
            _text.text = AccumulatedGold.ToString();
        }
    }
}

using Project.Scripts.UI;
using UnityEngine;

namespace Project.Scripts.Services
{
    public class FloatingTextService : IFloatingTextService
    {
        private const string ObjectPoolDamageText = "PoolDamageText";
        private const int Count = 4;
        private const bool IsAutoExpand = true;
    
        private readonly ObjectPool<FloatingTextView> _poolDamageText;

        public FloatingTextService(FloatingTextView textView)
        {
            _poolDamageText =
                new ObjectPool<FloatingTextView>(textView, Count, new GameObject(ObjectPoolDamageText).transform)
                {
                    AutoExpand = IsAutoExpand
                };
        }

        public void OnChangedFloatingText(string value, Transform target, Color targetColor)
        {
            ChangeText(value, target, targetColor);
        }

        private void ChangeText(string value, Transform target, Color targetColor)
        {
            FloatingTextView textView = _poolDamageText.GetFreeElement();
            textView.SetFloatingText(value, target, targetColor);
            textView.Show();
        }
    }
}
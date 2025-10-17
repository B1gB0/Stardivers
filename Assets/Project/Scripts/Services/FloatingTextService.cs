using Project.Scripts.UI.View;
using UnityEngine;

namespace Project.Scripts.Services
{
    public class FloatingTextService : IFloatingTextService
    {
        private const string ObjectPoolDamageText = "PoolDamageText";
        private const int Count = 4;
        private const bool IsAutoExpand = true;

        private ObjectPool<FloatingTextView> _poolDamageText;

        public void Init(FloatingTextView textView)
        {
            _poolDamageText =
                new ObjectPool<FloatingTextView>(textView, Count, new GameObject(ObjectPoolDamageText).transform)
                {
                    AutoExpand = IsAutoExpand
                };
        }

        public void OnChangedFloatingText(string value, Transform target, 
            FloatingTextViewType floatingTextViewType, Color color)
        {
            ChangeText(value, target, floatingTextViewType, color);
        }

        private void ChangeText(string value, Transform target, FloatingTextViewType floatingTextViewType, Color color)
        {
            FloatingTextView textView = _poolDamageText.GetFreeElement();
            textView.SetFloatingText(value, target, floatingTextViewType, color);
            textView.Show();
        }
    }
}
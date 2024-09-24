using Build.Game.Scripts;
using UnityEngine;

namespace Project.Scripts.UI
{
    public class FloatingDamageTextService
    {
        private const string ObjectPoolDamageText = "PoolDamageText";
        private const int Count = 4;
        private const bool IsAutoExpand = true;
    
        private readonly ObjectPool<FloatingDamageTextView> _poolDamageText;

        public FloatingDamageTextService(FloatingDamageTextView damageTextView)
        {
            _poolDamageText =
                new ObjectPool<FloatingDamageTextView>(damageTextView, Count, new GameObject(ObjectPoolDamageText).transform)
                {
                    AutoExpand = IsAutoExpand
                };
        }

        public void OnChangedDamageText(float damage, Transform target)
        {
            ChangeDamageText(damage, target);
        }

        private void ChangeDamageText(float damage, Transform target)
        {
            FloatingDamageTextView damageTextView = _poolDamageText.GetFreeElement();
            damageTextView.SetDamageText(damage, target);
            damageTextView.Show();
        }
    }
}
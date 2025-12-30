using Project.Scripts.UI.View;
using UnityEngine;

namespace Project.Scripts.Services
{
    public interface IFloatingTextService
    {
        public void OnChangedFloatingText(
            string value,
            Transform target,
            FloatingTextViewType floatingTextViewType,
            Color color);

        public void Init(FloatingTextView textView);
    }
}
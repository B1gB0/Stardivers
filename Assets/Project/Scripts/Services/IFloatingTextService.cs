using Project.Scripts.UI.View;
using UnityEngine;

namespace Project.Scripts.Services
{
    public interface IFloatingTextService
    {
        void OnChangedFloatingText(string value, Transform target, FloatingTextViewType floatingTextViewType, 
            Color color);
        void Init(FloatingTextView textView);
    }
}
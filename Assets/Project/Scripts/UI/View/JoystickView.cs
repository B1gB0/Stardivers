using DG.Tweening;
using UnityEngine;

namespace Project.Scripts.UI.View
{
    public class JoystickView : MonoBehaviour, IView
    {
        private void OnDestroy()
        {
            transform.DOKill();
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}

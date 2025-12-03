using UnityEngine;

namespace Project.Scripts.UI.View
{
    public class TutorialPointer : MonoBehaviour, IView
    {
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
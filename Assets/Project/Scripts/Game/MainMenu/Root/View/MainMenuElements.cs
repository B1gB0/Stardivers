using UnityEngine;

namespace Project.Scripts.Game.MainMenu.Root.View
{
    public class MainMenuElements : MonoBehaviour, IView
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
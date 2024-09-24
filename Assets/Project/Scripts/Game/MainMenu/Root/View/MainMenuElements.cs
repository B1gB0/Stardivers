using UnityEngine;

namespace Build.Game.Scripts.Game.Gameplay.GameplayRoot.View
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
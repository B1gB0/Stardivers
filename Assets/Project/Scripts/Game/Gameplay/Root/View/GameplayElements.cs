using UnityEngine;

namespace Project.Scripts.Game.Gameplay.Root.View
{
    public class GameplayElements : MonoBehaviour, IView
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
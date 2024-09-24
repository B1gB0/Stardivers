using UnityEngine;

namespace Build.Game.Scripts.Game.Gameplay.View
{
    public class GameplayElements : MonoBehaviour, IView
    {
        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(true);
        }
    }
}
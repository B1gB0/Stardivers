using System.Collections.Generic;
using Reflex.Attributes;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.UI.Panel
{
    public class EndGamePanel : MonoBehaviour, IView
    {
        [SerializeField] private Button _goToMainMenuButton;
        [SerializeField] private List<Image> _images;
        
        private PauseService _pauseService;

        public Button GoToMainMenuButton => _goToMainMenuButton;

        [Inject]
        public void Construct(PauseService pauseService)
        {
            _pauseService = pauseService;
        }

        public void OnChangeColor(Color color)
        {
            foreach (var image in _images)
            {
                image.color = color;
            }
        }
    
        public void Show()
        {
            _pauseService.StopGame();
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            _pauseService.PlayGame();
            gameObject.SetActive(false);
        }
    }
}

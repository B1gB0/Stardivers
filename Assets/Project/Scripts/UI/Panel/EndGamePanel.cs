using System.Collections.Generic;
using Reflex.Attributes;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.UI.Panel
{
    public class EndGamePanel : MonoBehaviour, IView
    {
        private const string VictoryLabelText = "Victory!";
        private const string DefeatLabelText = "Defeat!";
        
        private readonly Color _redColor = Color.red;
        private readonly Color _blueColor = Color.blue;

        [SerializeField] private Text _labelText;
        [SerializeField] private Button _goToMainMenuButton;
        [SerializeField] private Button _rebornPlayerButton;
        [SerializeField] private List<Image> _images;
        
        private PauseService _pauseService;

        public Button GoToMainMenuButton => _goToMainMenuButton;
        
        public Button RebornPlayerButton => _rebornPlayerButton;

        [Inject]
        public void Construct(PauseService pauseService)
        {
            _pauseService = pauseService;
        }

        private void OnEnable()
        {
            _goToMainMenuButton.onClick.AddListener(Hide);
            _rebornPlayerButton.onClick.AddListener(Hide);
        }

        private void OnDisable()
        {
            _goToMainMenuButton.onClick.RemoveListener(Hide);
            _rebornPlayerButton.onClick.RemoveListener(Hide);
        }

        public void SetVictoryPanel()
        {
            _labelText.text = VictoryLabelText;
            OnChangeColor(_blueColor);
        }

        public void SetDefeatPanel()
        {
            _labelText.text = DefeatLabelText;
            OnChangeColor(_redColor);
        }

        private void OnChangeColor(Color color)
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

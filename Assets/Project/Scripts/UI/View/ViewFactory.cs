using Project.Scripts.Experience;
using Project.Scripts.UI.Panel;
using Reflex.Attributes;
using UnityEngine;

namespace Project.Scripts.UI.View
{
    public class ViewFactory : MonoBehaviour
    {
        [SerializeField] private HealthBar _healthBarTemplate;
        [SerializeField] private FloatingTextView textViewTemplate;
        [SerializeField] private ProgressRadialBar _progressRadialBarPlaneTemplate;
        [SerializeField] private LevelUpPanel _levelUpPanelTemplate;
        [SerializeField] private EndGamePanel _endGamePanelTemplate;
        [SerializeField] private Timer _timer;
        [SerializeField] private AdviserMessagePanel _adviserMessagePanel;
        [SerializeField] private GoldView _goldView;
        
        private AudioSoundsService _audioSoundsService;
        private PauseService _pauseService;

        [Inject]
        public void Construct(AudioSoundsService audioSoundsService, PauseService pauseService)
        {
            _audioSoundsService = audioSoundsService;
            _pauseService = pauseService;
        }

        public HealthBar CreateHealthBar(Health.Health health)
        {
            HealthBar healthBar = Instantiate(_healthBarTemplate);
            healthBar.Construct(health);
            return healthBar;
        }

        public ProgressRadialBar CreateProgressBar(ExperiencePoints experiencePoints, Transform target)
        {
            ProgressRadialBar progressRadialBar = Instantiate(_progressRadialBarPlaneTemplate);
            progressRadialBar.Construct(experiencePoints, target);

            return progressRadialBar;
        }

        public FloatingTextView CreateDamageTextView()
        {
            FloatingTextView textView = Instantiate(textViewTemplate);
            return textView;
        }

        public LevelUpPanel CreateLevelUpPanel()
        {
            LevelUpPanel levelUpPanel = Instantiate(_levelUpPanelTemplate);
            return levelUpPanel;
        }

        public EndGamePanel CreateEndGamePanel()
        {
            EndGamePanel endGamePanel = Instantiate(_endGamePanelTemplate);
            return endGamePanel;
        }

        public Timer CreateTimer()
        {
            Timer timer = Instantiate(_timer);
            return timer;
        }

        public AdviserMessagePanel CreateAdviserMessagePanel()
        {
            AdviserMessagePanel adviserMessagePanel = Instantiate(_adviserMessagePanel);
            return adviserMessagePanel;
        }

        public GoldView CreateGoldView()
        {
            GoldView goldView = Instantiate(_goldView);
            return goldView;
        }
    }
}
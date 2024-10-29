using Build.Game.Scripts;
using Project.Scripts.Experience;
using Project.Scripts.Score;
using Project.Scripts.UI.Panel;
using Reflex.Attributes;
using UnityEngine;

namespace Project.Scripts.UI
{
    public class ViewFactory : MonoBehaviour
    {
        [SerializeField] private HealthBar _healthBarTemplate;
        [SerializeField] private FloatingDamageTextView _damageTextViewTemplate;
        [SerializeField] private ProgressRadialBar _progressRadialBarPlaneTemplate;
        [SerializeField] private LevelUpPanel _levelUpPanelTemplate;
        [SerializeField] private EndGamePanel _endGamePanelTemplate;
        [SerializeField] private Timer _timer;
        
        private AudioSoundsService _audioSoundsService;
        private PauseService _pauseService;

        [Inject]
        public void Construct(AudioSoundsService audioSoundsService, PauseService pauseService)
        {
            _audioSoundsService = audioSoundsService;
            _pauseService = pauseService;
        }

        public HealthBar CreateHealthBar(Health health)
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

        public FloatingDamageTextView CreateDamageTextView()
        {
            FloatingDamageTextView damageTextView = Instantiate(_damageTextViewTemplate);
            return damageTextView;
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
    }
}
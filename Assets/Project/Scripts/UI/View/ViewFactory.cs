using Build.Game.Scripts;
using Project.Scripts.Score;
using UnityEngine;

namespace Project.Scripts.UI
{
    public class ViewFactory : MonoBehaviour
    {
        [SerializeField] private HealthBar _healthBarTemplate;
        [SerializeField] private FloatingDamageTextView _damageTextViewTemplate;
        [SerializeField] private ProgressRadialBar _progressRadialBarPlaneTemplate;
        [SerializeField] private LevelUpPanel _levelUpPanelTemplate;

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
    }
}
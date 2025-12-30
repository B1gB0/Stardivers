using Project.Scripts.Experience;
using Project.Scripts.Game.Constant;
using UnityEngine;
using YG;

namespace Project.Scripts.UI.View
{
    public class ProgressRadialBar : RadialBar
    {
        private const string LevelRu = "УР ";
        private const string LevelEn = "LVL ";
        private const string LevelTr = "SEV ";

        private const float StartValueLevel = 0f;
        private const float Height = 0.1f;
        private const int StepLevel = 1;

        private ExperiencePoints _experiencePoints;
        private Transform _target;
        private int _currentLevel;

        private void OnEnable()
        {
            ChangeText();

            _experiencePoints.ValueIsChanged += OnChangeValue;
            _experiencePoints.ProgressBarLevelIsUpgraded += UpgradeProgressBarLevel;

            _experiencePoints.LoadLevel();
        }

        private void FixedUpdate()
        {
            transform.position = new Vector3(_target.position.x, Height, _target.position.z);
        }

        private void OnDisable()
        {
            _experiencePoints.ValueIsChanged -= OnChangeValue;
            _experiencePoints.ProgressBarLevelIsUpgraded -= UpgradeProgressBarLevel;
        }

        public void Construct(ExperiencePoints experiencePoints, Transform target)
        {
            _experiencePoints = experiencePoints;
            _target = target;
        }

        public void ChangeText()
        {
            Text.text = YG2.lang switch
            {
                LocalizationCode.Ru => LevelRu + _currentLevel,
                LocalizationCode.En => LevelEn + _currentLevel,
                LocalizationCode.Tr => LevelTr + _currentLevel,
                _ => Text.text
            };
        }

        private void UpgradeProgressBarLevel(int level, float targetValue, float maxValue)
        {
            _currentLevel = StepLevel + level;

            ChangeText();

            OnChangeValue(StartValueLevel, targetValue, maxValue);
        }
    }
}
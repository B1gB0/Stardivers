using Project.Scripts.Experience;
using UnityEngine;
using YG;

namespace Project.Scripts.UI.View
{
    public class ProgressRadialBar : RadialBar
    {
        private const string Ru = "ru";
        private const string En = "en";
        private const string Tr = "tr";

        private const string LevelRu = "УР ";
        private const string LevelEn = "LVL ";
        private const string LevelTr = "SEV ";
        
        private readonly float _startValueLevel = 0f;
        private readonly float _height = 0.02f;
        private readonly int _stepLevel = 1;
        
        private ExperiencePoints _experiencePoints;
        private Transform _target;
        private int _currentLevel;

        public void Construct(ExperiencePoints experiencePoints, Transform target)
        {
            _experiencePoints = experiencePoints;
            _target = target;
        }

        private void OnEnable()
        {
            _currentLevel += _stepLevel;
            ChangeText();
            
            _experiencePoints.ValueIsChanged += OnChangeValue;
            _experiencePoints.ProgressBarLevelIsUpgraded += UpgradeProgressBarLevel;
        }

        private void Update()
        {
            transform.position = new Vector3(_target.position.x, _height, _target.position.z);
        }

        private void OnDisable()
        {
            _experiencePoints.ValueIsChanged -= OnChangeValue;
            _experiencePoints.ProgressBarLevelIsUpgraded -= UpgradeProgressBarLevel;
        }

        private void UpgradeProgressBarLevel(int level, float targetValue, float maxValue)
        {
            _currentLevel += _stepLevel;

            ChangeText();
            
            UpdateLevelValue(_startValueLevel, maxValue);
            OnChangeValue(_startValueLevel, targetValue, maxValue);
        }

        public void ChangeText()
        {
            text.text = YG2.lang switch
            {
                Ru => LevelRu + _currentLevel,
                En => LevelEn + _currentLevel,
                Tr => LevelTr + _currentLevel,
                _ => text.text
            };
        }
    }
}
using System;
using Cinemachine;
using Project.Scripts.Experience;
using Project.Scripts.Score;
using UnityEngine;

namespace Project.Scripts.UI
{
    public class ProgressRadialBar : RadialBar
    {
        private readonly float _startValueLevel = 0f;
        private readonly float _height = 0.02f;
        private readonly int _stepLevel = 1;
        
        private ExperiencePoints _experiencePoints;
        private Transform _target;

        public void Construct(ExperiencePoints experiencePoints, Transform target)
        {
            _experiencePoints = experiencePoints;
            _target = target;
        }

        private void OnEnable()
        {
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
            level += _stepLevel;
            text.text = "LVL " + level;
            
            UpdateLevelValue(_startValueLevel, maxValue);
            OnChangeValue(_startValueLevel, targetValue, maxValue);
        }
    }
}
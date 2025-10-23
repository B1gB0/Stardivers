using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Project.Scripts.ECS.Data;
using Project.Scripts.ECS.EntityActors;
using Project.Scripts.UI.Panel;

namespace Project.Scripts.Experience
{
    public class ExperiencePoints
    {
        private const int DefaultLevel = 0;
        private const int CorrectFactorCounter = 1;
        private const float DelayLevelUp = 0.2f;

        private readonly ExperienceScoreActorVisitor _experienceScoreActorVisitor = new();
        private readonly PlayerProgressionInitData _playerProgression;
        private readonly Queue<int> _pendingLevelUps = new ();
        private readonly LevelUpPanel _levelUpPanel;
        
        private bool _isLevelUpProcessing;

        public ExperiencePoints(PlayerProgressionInitData playerProgression, LevelUpPanel levelUpPanel)
        {
            _playerProgression = playerProgression;
            _levelUpPanel = levelUpPanel;
            
            _counterLevel = DefaultLevel;
            _currentMaxValueOfLevel = _playerProgression.Levels[_counterLevel];
            _currentValue = TargetExperienceValue;
        }

        private int _currentMaxValueOfLevel;
        private int _currentValue;
        private int _currentLevel;
        private int _counterLevel;
        private int _newValue;

        private int TargetExperienceValue => _experienceScoreActorVisitor.AccumulatedExperience;

        public event Action<float, float, float> ValueIsChanged;
        public event Action<int, float, float> ProgressBarLevelIsUpgraded;
        public event Action<int> CurrentLevelIsUpgraded;

        public void OnKill(IAcceptable experience)
        {
            experience.AcceptScore(_experienceScoreActorVisitor);

            if (_counterLevel > _playerProgression.Levels.Count - CorrectFactorCounter) return;
            
            while (_counterLevel < _playerProgression.Levels.Count - CorrectFactorCounter && 
                   TargetExperienceValue >= _currentMaxValueOfLevel)
            {
                _counterLevel++;
                _currentMaxValueOfLevel = _playerProgression.Levels[_counterLevel];

                _newValue = Math.Abs(_currentMaxValueOfLevel - TargetExperienceValue);
                _experienceScoreActorVisitor.UpdateAccumulatedExperience(_newValue);

                ProgressBarLevelIsUpgraded?.Invoke(_counterLevel, TargetExperienceValue, _currentMaxValueOfLevel);
                _currentValue = TargetExperienceValue;
                
                for (int i = _currentLevel; i < _counterLevel; i++)
                {
                    _currentLevel++;
                    _pendingLevelUps.Enqueue(_currentLevel);
                }
            }
            
            if (TargetExperienceValue < _currentMaxValueOfLevel)
            {
                ValueIsChanged?.Invoke(_currentValue, TargetExperienceValue, _currentMaxValueOfLevel);
                _currentValue = TargetExperienceValue;
            }
            
            if (_pendingLevelUps.Count > DefaultLevel && !_isLevelUpProcessing)
            {
                ProcessLevelUps().Forget();
            }
        }

        private async UniTaskVoid ProcessLevelUps()
        {
            _isLevelUpProcessing = true;

            try
            {
                while (_pendingLevelUps.Count > DefaultLevel)
                {
                    int newLevel = _pendingLevelUps.Dequeue();
                
                    CurrentLevelIsUpgraded?.Invoke(newLevel);
                    
                    await UniTask.Delay(TimeSpan.FromSeconds(DelayLevelUp));
                }
            }
            finally
            {
                _isLevelUpProcessing = false;
            }
        }
        
        private async UniTask WaitForLevelUpPanelClose()
        {
            await UniTask.WaitUntil(() => _levelUpPanel.IsClosed);
        }
    }
}
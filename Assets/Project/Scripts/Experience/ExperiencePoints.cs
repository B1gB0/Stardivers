using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Project.Scripts.ECS.EntityActors;
using Project.Scripts.Services;
using YG;

namespace Project.Scripts.Experience
{
    public class ExperiencePoints
    {
        private const int DefaultLevel = 0;
        private const int CorrectFactorCounter = 1;

        private const float DelayLevelUp = 0.2f;

        private readonly ExperienceScoreActorVisitor _experienceScoreActorVisitor = new();
        private readonly Queue<int> _pendingLevelUps = new ();
        private readonly List<int> _playerLevels;

        private bool _isLevelUpProcessing;

        public ExperiencePoints(IPlayerService playerService)
        {
            _playerLevels = playerService.GetPlayerLevels();
            _currentMaxValueOfLevel = _playerLevels[_counterLevel];
            _counterLevel = DefaultLevel;
            _currentValue = TargetExperienceValue;
        }

        private int _currentMaxValueOfLevel;
        private int _currentValue;
        private int _currentLevel;
        private int _counterLevel;
        private int _newValue;
        
        public event Action<float, float, float> ValueIsChanged;
        public event Action<int, float, float> ProgressBarLevelIsUpgraded;
        public event Action<int> CurrentLevelIsUpgraded;
        
        public int AccumulatedKills => _experienceScoreActorVisitor.AccumulatedEnemyKills;
        public int AccumulatedScore => _experienceScoreActorVisitor.AccumulatedScore;
        private int TargetExperienceValue => _experienceScoreActorVisitor.AccumulatedExperience;

        public void OnKill(IAcceptable experience)
        {
            experience.AcceptScore(_experienceScoreActorVisitor);

            if (_counterLevel > _playerLevels.Count - CorrectFactorCounter) return;
            
            while (_counterLevel < _playerLevels.Count - CorrectFactorCounter && 
                   TargetExperienceValue >= _currentMaxValueOfLevel)
            {
                _counterLevel++;
                _currentMaxValueOfLevel = _playerLevels[_counterLevel];

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

            YG2.saves.ExperiencePointsValue = _currentValue;
            YG2.saves.CurrentLevel = _currentLevel;
        }

        public void LoadLevel()
        {
            _currentLevel = YG2.saves.CurrentLevel;
            _currentValue = YG2.saves.ExperiencePointsValue;
            _counterLevel = _currentLevel;
            ProgressBarLevelIsUpgraded?.Invoke(_currentLevel, _currentValue, _playerLevels[_currentLevel]);
        }

        public void ResetAccumulatedValues()
        {
            _experienceScoreActorVisitor.ResetAccumulatedValues();
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
    }
}
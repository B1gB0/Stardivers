using System;
using Project.Scripts.ECS.Data;
using Project.Scripts.ECS.EntityActors;

namespace Project.Scripts.Experience
{
    public class ExperiencePoints
    {
        private const int DefaultLevel = 0;
        
        private readonly ExperienceScoreActorVisitor _experienceScoreActorVisitor = new ();
        private readonly PlayerProgressionInitData _playerProgression;

        public ExperiencePoints(PlayerProgressionInitData playerProgression)
        {
            _playerProgression = playerProgression;
            _currentLevel = DefaultLevel;
            _currentMaxValueOfLevel = _playerProgression.Levels[_currentLevel];

            _currentValue = TargetValue;
        }

        private int _currentMaxValueOfLevel;
        private int _currentValue;
        private int _currentLevel;
        
        private int TargetValue => _experienceScoreActorVisitor.AccumulatedExperience;
        
        public event Action<float, float, float> ValueIsChanged;

        public event Action<int, float, float> LevelIsUpgraded;

        public event Action<int> CurrentLevelIsUpgraded;

        public void OnKill(IAcceptable experience)
        {
            experience.AcceptScore(_experienceScoreActorVisitor);
            
            if (_currentLevel > _playerProgression.Levels.Count - 1) return;
            
            if (TargetValue >= _currentMaxValueOfLevel)
            {
                _currentLevel++;
                _currentMaxValueOfLevel = _playerProgression.Levels[_currentLevel];
                
                int newValue = _currentMaxValueOfLevel - TargetValue;
                _experienceScoreActorVisitor.UpdateAccumulatedExperience(newValue);
                
                LevelIsUpgraded?.Invoke(_currentLevel, TargetValue, _currentMaxValueOfLevel);
                _currentValue = TargetValue;
                
                CurrentLevelIsUpgraded?.Invoke(_currentLevel);
            }
            else
            {
                ValueIsChanged?.Invoke( _currentValue, TargetValue, _currentMaxValueOfLevel );
                _currentValue = TargetValue;
            }
        }
    }
}
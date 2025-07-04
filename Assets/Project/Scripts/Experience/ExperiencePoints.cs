using System;
using Project.Scripts.ECS.Data;
using Project.Scripts.ECS.EntityActors;
using YG;

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
            _counterLevel = DefaultLevel;
            _currentLevel = DefaultLevel;
            _currentMaxValueOfLevel = _playerProgression.Levels[_counterLevel];

            _currentValue = TargetValue;
        }

        private int _currentMaxValueOfLevel;
        private int _currentValue;
        private int _counterLevel;
        private int _currentLevel;
        private int _newValue;
        
        private int TargetValue => _experienceScoreActorVisitor.AccumulatedExperience;
        
        public event Action<float, float, float> ValueIsChanged;

        public event Action<int, float, float> ProgressBarLevelIsUpgraded;

        public event Action<int> CurrentLevelIsUpgraded;

        public void OnKill(IAcceptable experience)
        {
            experience.AcceptScore(_experienceScoreActorVisitor);

            if (_counterLevel > _playerProgression.Levels.Count - 1) return;
            
            if (TargetValue >= _currentMaxValueOfLevel)
            {
                _counterLevel++;
                _currentMaxValueOfLevel = _playerProgression.Levels[_counterLevel];

                _newValue = Math.Abs(_currentMaxValueOfLevel - TargetValue);
                _experienceScoreActorVisitor.UpdateAccumulatedExperience(_newValue);

                ProgressBarLevelIsUpgraded?.Invoke(_counterLevel, TargetValue, _currentMaxValueOfLevel);
                _currentValue = TargetValue;

                if (TargetValue <= _currentMaxValueOfLevel)
                {
                    for (int i = _currentLevel; i < _counterLevel; i++)
                    {
                        if (YG2.isGameplaying)
                        {
                            _currentLevel++;
                            CurrentLevelIsUpgraded?.Invoke(_currentLevel);
                        }
                    }
                }
            }
            else
            {
                ValueIsChanged?.Invoke( _currentValue, TargetValue, _currentMaxValueOfLevel );
                _currentValue = TargetValue;
            }
        }
    }
}
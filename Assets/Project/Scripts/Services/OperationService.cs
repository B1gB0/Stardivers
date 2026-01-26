using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Project.Scripts.Levels;
using Reflex.Attributes;
using UnityEngine;

namespace Project.Scripts.Services
{
    public class OperationService : MonoBehaviour, IService
    {
        private const int DefaultNumberLevel = 2;

        private readonly Dictionary<int, string> _marsSceneLevels = new ();
        private readonly Dictionary<int, string> _mysteryPlanetSceneLevels = new ();

        private IDataBaseService _dataBaseService;

        [Inject]
        public void Construct(IDataBaseService dataBaseService)
        {
            _dataBaseService = dataBaseService;
        }

        [field: SerializeField] public List<Operation> Operations { get; private set; } = new ();

        public Operation CurrentOperation { get; private set; }
        public int CurrentNumberLevel { get; private set; }
        public bool IsInitiated { get; private set; }

        public UniTask Init()
        {
            if (IsInitiated)
                return UniTask.CompletedTask;

            foreach (var operation in Operations)
            {
                foreach (var operationData in _dataBaseService.Content.OperationsLocalization)
                {
                    if (operation.Id == operationData.Id)
                    {
                        operation.SetData(operationData);
                    }
                }
            }

            foreach (var marsSceneLevel in _dataBaseService.Content.MarsSceneLevels)
            {
                _marsSceneLevels.Add(marsSceneLevel.Number, marsSceneLevel.SceneName);
            }

            foreach (var mysteryPlanetSceneLevel in _dataBaseService.Content.MysteryPlanetSceneLevels)
            {
                _mysteryPlanetSceneLevels.Add(mysteryPlanetSceneLevel.Number, mysteryPlanetSceneLevel.SceneName);
            }

            IsInitiated = true;

            return UniTask.CompletedTask;
        }

        public void SetCurrentOperation(int index)
        {
            CurrentOperation = Operations[index];
            CurrentNumberLevel = DefaultNumberLevel;
        }

        public void SetCurrentNumberLevel(int numberLevel)
        {
            CurrentNumberLevel = numberLevel;
        }

        public string GetSceneNameByCurrentNumber()
        {
            return GetSceneNameByNumber(CurrentNumberLevel);
        }

        public string GetSceneNameByNumber(int number)
        {
            return CurrentOperation.Id switch
            {
                Game.Constant.Operations.Mars => _marsSceneLevels[number],
                Game.Constant.Operations.MysteryPlanet => _mysteryPlanetSceneLevels[number],
                _ => null
            };
        }
    }
}
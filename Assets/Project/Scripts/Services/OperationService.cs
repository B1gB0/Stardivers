using System.Collections.Generic;
using Project.Scripts.DataBase.Data;
using Project.Scripts.Levels;
using Reflex.Attributes;
using UnityEngine;
using UnityEngine.Profiling;

namespace Project.Scripts.Services
{
    public class OperationService : MonoBehaviour
    {
        private const int DefaultNumberLevel = 0;
        
        [field: SerializeField] public List<Operation> Operations { get; private set; } = new();

        private IDataBaseService _dataBaseService;
        
        public Operation CurrentOperation { get; private set; }
        
        public int CurrentNumberLevel { get; private set; }

        [Inject]
        public void Construct(IDataBaseService dataBaseService)
        {
            _dataBaseService = dataBaseService;
        }

        public void Init()
        {
            foreach (var operation in Operations)
            {
                foreach (var operationData in _dataBaseService.Content.Operations)
                {
                    if (operation.Id == operationData.Id)
                    {
                        operation.SetData(operationData.NameRu, operationData.NameEn, operationData.NameTr);
                    }
                }
            }
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
    }
}

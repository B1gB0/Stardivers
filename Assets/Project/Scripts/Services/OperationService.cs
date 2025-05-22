using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Project.Scripts.Levels;
using Reflex.Attributes;
using UnityEngine;

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

        public async UniTask Init()
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

using System.Collections.Generic;
using Project.Scripts.Levels;
using UnityEngine;
using UnityEngine.Profiling;

namespace Project.Scripts.Services
{
    public class OperationService : MonoBehaviour
    {
        private const int DefaultNumberLevel = 0;
        
        [field: SerializeField] public List<Operation> Operations { get; private set; } = new();
        
        public Operation CurrentOperation { get; private set; }
        
        public int CurrentNumberLevel { get; private set; }
        
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

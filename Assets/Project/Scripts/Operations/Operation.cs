using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.Operations
{
    [CreateAssetMenu(menuName = "Operations/New Operation")]
    public class Operation : ScriptableObject
    {
        [field: SerializeField] public List<MapInitData> Maps { get; private set; } = new();
        
        [field: SerializeField] public string Name { get; private set; }
        
        [field: SerializeField] public string CodeName { get; private set; }

        [field: SerializeField] public Sprite Image { get; private set; }
        
    }
}
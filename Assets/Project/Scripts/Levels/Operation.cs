using System.Collections.Generic;
using Project.Scripts.ECS.Data;
using UnityEngine;

namespace Project.Scripts.Levels
{
    [CreateAssetMenu(menuName = "Operations/New Operation")]
    public class Operation : ScriptableObject
    {
        [field: SerializeField] public List<LevelInitData> Maps { get; private set; } = new();
        [field: SerializeField] public string Name { get; private set; }
        
        [field: SerializeField] public string CodeName { get; private set; }
        [field: SerializeField] public Sprite Image { get; private set; }
        
    }
}
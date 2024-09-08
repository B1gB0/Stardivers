using System.Collections.Generic;
using UnityEngine;

namespace Project.Scripts.ECS.Data
{
    [CreateAssetMenu(menuName = "InitData/PlayerProgressionInitData")]
    public class PlayerProgressionInitData : InitData
    {
        [field: SerializeField] public List<int> Levels { get; private set; }
    }
}
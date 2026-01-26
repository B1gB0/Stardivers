using System;
using UnityEngine;

namespace Project.Scripts.DataBase.Data
{
    [Serializable]
    public class PlayerLevelData
    {
        [SerializeField] private int _requiredExperience;
        
        public int RequiredExperience => _requiredExperience;
    }
}
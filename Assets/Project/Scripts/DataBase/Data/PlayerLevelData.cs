using System;
using UnityEngine;

namespace Project.Scripts.DataBase.Data
{
    [Serializable]
    public class PlayerLevelData
    {
        [SerializeField] private int _id;
        [SerializeField] private int _requiredExperience;

        public int Id => _id;
        public int RequiredExperience => _requiredExperience;
    }
}
using System;
using UnityEngine;

namespace Project.Scripts.DataBase.Data
{
    [Serializable]
    public class PlayerLevelData
    {
        [SerializeField] private string _id;
        [SerializeField] private int _requiredExperience;

        public string Id => _id;
        public int RequiredExperience => _requiredExperience;
    }
}
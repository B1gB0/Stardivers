using System;
using Project.Scripts.ECS.EntityActors;
using UnityEngine;

namespace Project.Scripts.DataBase.Data
{
    [Serializable]
    public class CoreData
    {
        [SerializeField] private string _id;
        [SerializeField] private CoreType _type;
        [SerializeField] private float _health;
        [SerializeField] private int _experience;
        [SerializeField] private int _score;
        [SerializeField] private float _crystalValue;
        
        public string Id => _id;
        public CoreType Type => _type;
        public float Health => _health;
        public int Experience => _experience;
        public int Score => _score;
        public float CrystalValue => _crystalValue;
    }
}
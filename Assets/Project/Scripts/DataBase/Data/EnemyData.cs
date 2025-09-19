using System;
using Project.Scripts.ECS.EntityActors;
using UnityEngine;

namespace Project.Scripts.DataBase.Data
{
    [Serializable]
    public class EnemyData
    {
        [SerializeField] private string _id;
        [SerializeField] private EnemyAlienActorType _type;
        [SerializeField] private float _health;
        [SerializeField] private float _speed;
        [SerializeField] private float _damage;

        public string Id => _id;
        public EnemyAlienActorType Type => _type;
        public float Health => _health;
        public float Speed => _speed;
        public float Damage => _damage;
    }
}
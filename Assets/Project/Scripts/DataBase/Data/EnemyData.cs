using System;
using Project.Scripts.ECS.EntityActors;
using UnityEngine;

namespace Project.Scripts.DataBase.Data
{
    [Serializable]
    public class EnemyData
    {
        [SerializeField] private string _id;
        [SerializeField] private EnemyActorType _type;
        [SerializeField] private float _health;
        [SerializeField] private float _speed;
        [SerializeField] private float _damage;
        [SerializeField] private float _fireRate;
        [SerializeField] private float _rangeAttack;
        [SerializeField] private int _experience;
        [SerializeField] private int _score;

        public string Id => _id;
        public EnemyActorType Type => _type;
        public float Health => _health;
        public float Speed => _speed;
        public float Damage => _damage;
        public float FireRate => _fireRate;
        public float RangeAttack => _rangeAttack;
        public int Experience => _experience;
        public int Score => _score;
    }
}
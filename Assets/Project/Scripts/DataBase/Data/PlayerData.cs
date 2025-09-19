using System;
using Project.Scripts.ECS.EntityActors;
using UnityEngine;

namespace Project.Scripts.DataBase.Data
{
    [Serializable]
    public class PlayerData
    {
        [SerializeField] private string _id;
        [SerializeField] private PlayerActorType _type;
        [SerializeField] private float _health;
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _rotationSpeed;
        [SerializeField] private float _diggingSpeed;

        public string Id => _id;
        public PlayerActorType Type => _type;
        public float Health => _health;
        public float MoveSpeed => _moveSpeed;
        public float RotationSpeed => _rotationSpeed;
        public float DiggingSpeed => _diggingSpeed;
    }
}
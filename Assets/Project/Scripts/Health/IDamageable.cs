using System;
using UnityEngine;

namespace Build.Game.Scripts
{
    public interface IDamageable
    {
        event Action Die;

        void TakeDamage(float damage);
    }
}

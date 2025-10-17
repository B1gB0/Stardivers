using System;

namespace Project.Scripts.Health
{
    public interface IDamageable
    {
        event Action Die;
        event Action<Health> DieHealth;

        void TakeDamage(float damage);
    }
}

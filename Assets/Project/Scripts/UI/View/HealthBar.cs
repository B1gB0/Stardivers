namespace Project.Scripts.UI.View
{
    public class HealthBar : Bar
    {
        private Health.Health _health;

        public void Construct(Health.Health health)
        {
            _health = health;
        }

        private void OnEnable()
        {
            _health.Die += OnDie;
            _health.HealthChanged += OnChangedValues;
        }

        private void OnDisable()
        {
            _health.Die -= OnDie;
            _health.HealthChanged -= OnChangedValues;
        }

        private void OnDie()
        {
            Hide();
        }

        private void OnChangedValues(float currentHealth, float maxHealth, float targetHealth)
        {
            SetValues(currentHealth, maxHealth, targetHealth);
        }
    }
}

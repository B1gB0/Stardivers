namespace Project.Scripts.Weapon.Characteristics
{
    public class MineCharacteristics : Characteristics
    {
        public override void SetStartingCharacteristics()
        {
            fireRate = 5f;
            damage = 20f;
            explosionRadius = 5f;
            maxCountBullets = 1;
        }

        public override void ApplyImprovement(CharacteristicType type ,float factor)
        {
            switch (type)
            {
                case CharacteristicType.Damage :
                    IncreaseDamage(factor);
                    break;
                case CharacteristicType.FireRate :
                    IncreaseFireRate(factor);
                    break;
                case CharacteristicType.ExplosionRadius :
                    IncreaseExplosionRadius(factor);
                    break;
            }
        }
    }
}
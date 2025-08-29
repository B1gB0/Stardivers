namespace Project.Scripts.Weapon.Characteristics
{
    public class ChainLightningGunCharacteristics : Characteristics
    {
        public override void SetStartingCharacteristics()
        {
            rangeAttack = 6f;
            fireRate = 3f;
            damage = 10f;
            maxCountBullets = 4;
            reloadTime = 5f;
        }

        public override void ApplyImprovement(CharacteristicType type, float factor)
        {
            switch (type)
            {
                case CharacteristicType.Damage:
                    IncreaseDamage(factor);
                    break;
                case CharacteristicType.FireRate:
                    IncreaseFireRate(factor);
                    break;
                case CharacteristicType.RangeAttack:
                    IncreaseRangeAttack(factor);
                    break;
                case CharacteristicType.ReloadTime:
                    IncreaseReloadVelocity(factor);
                    break;
                case CharacteristicType.MaxCountBullets:
                    IncreaseMaxCountBullets((int)factor);
                    break;
            }
        }
    }
}
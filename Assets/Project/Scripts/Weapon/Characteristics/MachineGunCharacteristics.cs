using System.Collections;
using System.Collections.Generic;
using Project.Game.Scripts;
using UnityEngine;

public class MachineGunCharacteristics
{
    public float RangeAttack { get; private set; } = 5f;

    public float FireRate { get; private set; } = 3f;

    public float BulletSpeed { get; private set; } = 10f;

    public float Damage { get; private set; } = 18f;
    
    public int MaxCountShots { get; private set; } = 16;
    
    public float ReloadTime { get; private set; } = 6f;
    
    public void ApplyImprovement(CharacteristicsTypes type ,float factor)
    {
        switch (type)
        {
            case CharacteristicsTypes.Damage :
                IncreaseDamage(factor);
                break;
            case CharacteristicsTypes.FireRate :
                IncreaseFireRate(factor);
                break;
            case CharacteristicsTypes.ProjectileSpeed :
                IncreaseBulletSpeed(factor);
                break;
            case CharacteristicsTypes.RangeAttack :
                IncreaseRangeAttack(factor);
                break;
            case CharacteristicsTypes.ReloadTime :
                IncreaseReloadVelocity(factor);
                break;
        }
    }
        
    private void IncreaseDamage(float damageFactor)
    {
        Damage += Mathf.Round(Damage * damageFactor);
    }

    private void IncreaseFireRate(float fireRateFactor)
    {
        FireRate -= Mathf.Round(FireRate * fireRateFactor);
    }

    private void IncreaseBulletSpeed(float bulletSpeedFactor)
    {
        BulletSpeed += Mathf.Round(BulletSpeed * bulletSpeedFactor);
    }

    private void IncreaseRangeAttack(float rangeAttackFactor)
    {
        RangeAttack += Mathf.Round(RangeAttack * rangeAttackFactor);
    }
    
    private void IncreaseReloadVelocity(float reloadTimeFactor)
    {
        ReloadTime += Mathf.Round(ReloadTime * reloadTimeFactor);
    }
}

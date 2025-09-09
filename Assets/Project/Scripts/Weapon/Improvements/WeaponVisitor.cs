using Project.Scripts.Weapon.Characteristics;
using Project.Scripts.Weapon.Player;

namespace Project.Scripts.Weapon.Improvements
{
    public class WeaponVisitor : IWeaponVisitor
    {
        public void Visit(Gun gun, CharacteristicType type, float value)
        {
            gun.GunCharacteristics.ApplyImprovement(type, value);
        }

        public void Visit(FourBarrelMachineGun fourBarrelMachineGun, CharacteristicType type, float value)
        {
            fourBarrelMachineGun.MachineGunCharacteristics.ApplyImprovement(type, value);
        }

        public void Visit(Mines mines, CharacteristicType type, float value)
        {
            mines.MineCharacteristics.ApplyImprovement(type, value);
        }

        public void Visit(FragGrenades fragGrenades, CharacteristicType type, float value)
        {
            fragGrenades.FragGrenadeCharacteristics.ApplyImprovement(type, value);
        }

        public void Visit(MachineGun machineGun, CharacteristicType type, float value)
        {
            machineGun.MachineGunCharacteristics.ApplyImprovement(type, value);
        }

        public void Visit(ChainLightningGun chainLightningGun, CharacteristicType type, float value)
        {
            chainLightningGun.ChainLightningGunCharacteristics.ApplyImprovement(type, value);
        }
    }
}
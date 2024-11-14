using Project.Scripts.Weapon.Improvements;
using Project.Scripts.Weapon.Player;

namespace Project.Game.Scripts.Improvements
{
    public class WeaponVisitor : IWeaponVisitor
    {
        public void Visit(Gun gun, CharacteristicsTypes type, float value)
        {
            gun.GunCharacteristics.ApplyImprovement(type, value);
        }

        public void Visit(FourBarrelMachineGun fourBarrelMachineGun, CharacteristicsTypes type, float value)
        {
            fourBarrelMachineGun.MachineGunCharacteristics.ApplyImprovement(type, value);
        }

        public void Visit(Mines mines, CharacteristicsTypes type, float value)
        {
            mines.MineCharacteristics.ApplyImprovement(type, value);
        }

        public void Visit(FragGrenades fragGrenades, CharacteristicsTypes type, float value)
        {
            fragGrenades.FragGrenadeCharacteristics.ApplyImprovement(type, value);
        }

        public void Visit(MachineGun machineGun, CharacteristicsTypes type, float value)
        {
            machineGun.MachineGunCharacteristics.ApplyImprovement(type, value);
        }
    }
}
using Project.Scripts.Weapon.CharacteristicsOfWeapon;
using Project.Scripts.Weapon.Player;

namespace Project.Scripts.Weapon.Improvements
{
    public interface IWeaponVisitor
    {
        public void Visit(Gun gun, CharacteristicType type, float value);
        public void Visit(FourBarrelMachineGun fourBarrelMachineGun, CharacteristicType type, float value);
        public void Visit(Mines mines, CharacteristicType type, float value);
        public void Visit(FragGrenades fragGrenades, CharacteristicType type, float value);
        public void Visit(MachineGun machineGun, CharacteristicType type, float value);
        public void Visit(ChainLightningGun chainLightningGun, CharacteristicType type, float value);
    }
}
using Project.Game.Scripts;
using Project.Scripts.Weapon.Characteristics;
using Project.Scripts.Weapon.Player;

namespace Project.Scripts.Weapon.Improvements
{
    public interface IWeaponVisitor
    {
        void Visit(Gun gun, CharacteristicType type, float value);
    
        void Visit(FourBarrelMachineGun fourBarrelMachineGun, CharacteristicType type, float value);
        
        void Visit(Mines mines, CharacteristicType type, float value);
        
        void Visit(FragGrenades fragGrenades, CharacteristicType type, float value);
        
        void Visit(MachineGun machineGun, CharacteristicType type, float value);
    }
}
using Project.Game.Scripts;
using Project.Scripts.Weapon.Player;

namespace Project.Scripts.Weapon.Improvements
{
    public interface IWeaponVisitor
    {
        void Visit(Gun gun, CharacteristicsTypes type, float value);
    
        void Visit(FourBarrelMachineGun fourBarrelMachineGun, CharacteristicsTypes type, float value);
        
        void Visit(Mines mines, CharacteristicsTypes type, float value);
        
        void Visit(FragGrenades fragGrenades, CharacteristicsTypes type, float value);
        
        void Visit(MachineGun machineGun, CharacteristicsTypes type, float value);
    }
}
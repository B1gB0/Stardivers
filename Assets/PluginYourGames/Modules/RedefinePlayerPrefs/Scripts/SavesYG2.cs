using System.Collections.Generic;
using Project.Scripts.Weapon.CharacteristicsOfWeapon;

namespace YG
{
    public partial class SavesYG
    {
        public int Gold;
        public int RedCrystal;
        public int CountKilledEnemy;
        
        public GunCharacteristics GunCharacteristics;
        public MachineGunCharacteristics MachineGunCharacteristics;
        public MachineGunCharacteristics FourBarrelMachineGunCharacteristics;
        public ChainLightningGunCharacteristics ChainLightningGunCharacteristics;
        public FragGrenadeCharacteristics FragGrenadeCharacteristics;
        public MineCharacteristics MineCharacteristics;

        public List<string> stringKeys = new List<string>();
        public List<string> stringValues = new List<string>();

        public List<string> floatKeys = new List<string>();
        public List<float> floatValues = new List<float>();

        public List<string> intKeys = new List<string>();
        public List<int> intValues = new List<int>();
    }
}

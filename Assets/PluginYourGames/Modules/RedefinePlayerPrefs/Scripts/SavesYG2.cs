using System.Collections.Generic;
using Project.Scripts.DataBase.Data;

namespace YG
{
    public partial class SavesYG
    {
        public int Gold;
        public int RedCrystal;
        public int AcumulatedScore;
        public int ExperiencePointsValue;
        public int CurrentLevel;
        public bool isMarsOperationUnlock = true;
        public bool isMysteryPlanetUnlock;
        
        public CharacteristicsWeaponData GunCharacteristics;
        public CharacteristicsWeaponData MachineGunCharacteristics;
        public CharacteristicsWeaponData FourBarrelMachineGunCharacteristics;
        public CharacteristicsWeaponData ChainLightningGunCharacteristics;
        public CharacteristicsWeaponData FragGrenadeCharacteristics;
        public CharacteristicsWeaponData MinesCharacteristics;

        public List<string> stringKeys = new List<string>();
        public List<string> stringValues = new List<string>();

        public List<string> floatKeys = new List<string>();
        public List<float> floatValues = new List<float>();

        public List<string> intKeys = new List<string>();
        public List<int> intValues = new List<int>();
    }
}

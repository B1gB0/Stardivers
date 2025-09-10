using System;
using System.Collections.Generic;
using NorskaLib.Spreadsheets;
using Project.Scripts.DataBase.Data;

namespace Project.Scripts.DataBase
{
    [Serializable]
    public class SpreadsheetContent
    {
        [SpreadsheetPage("OperationsLocalization")] public List<OperationLocalizationData> OperationsLocalization;
        [SpreadsheetPage("CharacteristicsLocalization")] public List<CharacteristicsLocalizationData>
            CharacteristicsLocalization;
        [SpreadsheetPage("CharacteristicsWeapon")] public List<CharacteristicsWeaponData> CharacteristicsWeaponsData;
        [SpreadsheetPage("WeaponsLocalization")] public List<WeaponLocalizationData> WeaponsLocalization;
        [SpreadsheetPage("PlayerLevels")] public List<PlayerLevelData> PlayerLevels;
        [SpreadsheetPage("Improvements")] public List<ImprovementData> Improvements;
        [SpreadsheetPage("MarsSceneLevels")] public List<SceneLevelData> MarsSceneLevels;
        [SpreadsheetPage("MysteryPlanetSceneLevels")] public List<SceneLevelData> MysteryPlanetSceneLevels;
    }
}
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
        [SpreadsheetPage("WeaponsLocalization")] public List<WeaponLocalizationData> WeaponsLocalization;
        [SpreadsheetPage("CharacteristicsWeapon")] public List<CharacteristicsWeaponData> CharacteristicsWeaponsData;
        [SpreadsheetPage("Improvements")] public List<ImprovementData> Improvements;
        [SpreadsheetPage("PlayerLevels")] public List<PlayerLevelData> PlayerLevels;
        [SpreadsheetPage("Players")] public List<PlayerData> Players;
        [SpreadsheetPage("Enemies")] public List<EnemyData> Enemies;
        [SpreadsheetPage("MarsSceneLevels")] public List<SceneLevelData> MarsSceneLevels;
        [SpreadsheetPage("MysteryPlanetSceneLevels")] public List<SceneLevelData> MysteryPlanetSceneLevels;
        [SpreadsheetPage("LevelTexts")] public List<LevelTextData> LevelTexts;
        [SpreadsheetPage("Cores")] public List<CoreData> Cores;
    }
}
﻿using System;
using System.Collections.Generic;
using NorskaLib.Spreadsheets;
using Project.Scripts.DataBase.Data;

namespace Project.Scripts.DataBase
{
    [Serializable]
    public class SpreadsheetContent
    {
        [SpreadsheetPage("Operations")] public List<OperationData> Operations;
        [SpreadsheetPage("Characteristics")] public List<CharacteristicsData> Characteristics;
        [SpreadsheetPage("Weapons")] public List<WeaponData> Weapons;
    }
}
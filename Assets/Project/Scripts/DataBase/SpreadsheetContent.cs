using System;
using System.Collections.Generic;
using NorskaLib.Spreadsheets;
using Project.Scripts.DataBase.Data;

namespace Project.Scripts.DataBase
{
    [Serializable]
    public class SpreadsheetContent
    {
        [SpreadsheetPage("Localization")] public List<LocalizationData> LocalizationTexts;
    }
}
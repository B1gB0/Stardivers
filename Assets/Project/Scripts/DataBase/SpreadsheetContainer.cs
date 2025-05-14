using NorskaLib.Spreadsheets;
using UnityEngine;

namespace Project.Scripts.DataBase
{
    [CreateAssetMenu(fileName = "SpreadsheetContainer", menuName = "SpreadsheetContainer")]
    public class SpreadsheetContainer : SpreadsheetsContainerBase
    {
        [SpreadsheetContent]
        [SerializeField] private SpreadsheetContent _content;

        public SpreadsheetContent Content => _content;
    }
}
using System;

namespace Project.Scripts.DataBase
{
    public class DataBaseService : IDataBaseService
    {
        public SpreadsheetContainer Data { get; }
        public SpreadsheetContent Content => Data.Content;
        
        public void Init()
        {
            
        }

        public event Action OnLoaded;
    }
}
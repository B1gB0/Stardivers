using System;

namespace Project.Scripts.DataBase
{
    public interface IDataBaseService
    {
        SpreadsheetContainer Data { get; }
        SpreadsheetContent Content { get; }
        void Init();
        event Action OnLoaded;
    }
}
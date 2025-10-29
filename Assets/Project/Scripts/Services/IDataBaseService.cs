using System;
using Project.Scripts.DataBase;

namespace Project.Scripts.Services
{
    public interface IDataBaseService : IService
    {
        SpreadsheetContainer Data { get; }
        SpreadsheetContent Content { get; }
        public event Action OnDataLoaded;
    }
}
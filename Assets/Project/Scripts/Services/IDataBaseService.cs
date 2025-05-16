using System;
using Cysharp.Threading.Tasks;
using Project.Scripts.DataBase;

namespace Project.Scripts.Services
{
    public interface IDataBaseService
    {
        SpreadsheetContainer Data { get; }
        SpreadsheetContent Content { get; }
        UniTask Init();
        event Action OnDataLoaded;
    }
}
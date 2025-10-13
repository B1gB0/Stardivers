using System;
using Cysharp.Threading.Tasks;
using Project.Scripts.DataBase;

namespace Project.Scripts.Services
{
    public interface IDataBaseService
    {
        SpreadsheetContainer Data { get; }
        SpreadsheetContent Content { get; }
        public UniTask Init();
        public event Action OnDataLoaded;
    }
}
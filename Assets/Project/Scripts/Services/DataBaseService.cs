using System;
using Cysharp.Threading.Tasks;
using Project.Scripts.DataBase;

namespace Project.Scripts.Services
{
    public class DataBaseService : IDataBaseService
    {
        public SpreadsheetContainer Data { get; private set; }
        public SpreadsheetContent Content => Data.Content;
        
        public async UniTask Init()
        {
            
        }

        public event Action OnLoaded;
    }
}
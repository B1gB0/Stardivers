using System;
using Cysharp.Threading.Tasks;
using Project.Scripts.DataBase;
using Reflex.Attributes;

namespace Project.Scripts.Services
{
    public class DataBaseService : Service, IDataBaseService
    {
        private const string DataContainer = nameof(DataContainer);

        private IResourceService _resourceService;

        public SpreadsheetContainer Data { get; private set; }
        public SpreadsheetContent Content => Data.Content;

        public event Action OnDataLoaded;

        [Inject]
        private void Construct(IResourceService resourceService)
        {
            _resourceService = resourceService;
        }

        public override async UniTask Init()
        {
            if (Data != null)
                return;

            Data = await _resourceService.Load<SpreadsheetContainer>(DataContainer);
            OnDataLoaded?.Invoke();
        }
    }
}
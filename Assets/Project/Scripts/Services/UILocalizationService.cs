using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Project.Scripts.DataBase.Data;
using Project.Scripts.UI;
using Reflex.Attributes;

namespace Project.Scripts.Services
{
    public class UILocalizationService : IUILocalizationService
    {
        private readonly Dictionary<UITextType, UILocalizationData> _uiLocalizationData = new();

        private IDataBaseService _dataBaseService;

        public bool IsInitiated { get; private set; }

        [Inject]
        public void Construct(IDataBaseService dataBaseService)
        {
            _dataBaseService = dataBaseService;
        }

        public UniTask Init()
        {
            if (IsInitiated)
                return UniTask.CompletedTask;

            foreach (var data in _dataBaseService.Content.UILocalizationData)
            {
                _uiLocalizationData.TryAdd(data.UITextType, data);
            }

            IsInitiated = true;

            return UniTask.CompletedTask;
        }

        public UILocalizationData GetLevelTextData(UITextType type)
        {
            return _uiLocalizationData[type];
        }
    }
}
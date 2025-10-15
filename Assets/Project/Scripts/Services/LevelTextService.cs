using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Project.Scripts.DataBase.Data;
using Project.Scripts.Levels;
using Reflex.Attributes;

namespace Project.Scripts.Services
{
    public class LevelTextService : Service, ILevelTextService
    {
        private readonly Dictionary<string, LevelTextData> _levelText = new();

        private IDataBaseService _dataBaseService;

        [Inject]
        private void Construct(IDataBaseService dataBaseService)
        {
            _dataBaseService = dataBaseService;
        }

        public override UniTask Init()
        {
            foreach (var levelTextData in _dataBaseService.Content.LevelTexts)
            {
                _levelText.Add(levelTextData.Id, levelTextData);
            }

            return UniTask.CompletedTask;
        }

        public LevelTextData GetLevelTextData(string sceneName, LevelTextsType type)
        {
            return _levelText.Values
                .FirstOrDefault(data => data.SceneName == sceneName && data.Type == type);
        }
    }
}
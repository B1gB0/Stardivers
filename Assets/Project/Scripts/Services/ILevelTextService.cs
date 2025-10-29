using Project.Scripts.DataBase.Data;
using Project.Scripts.Levels;

namespace Project.Scripts.Services
{
    public interface ILevelTextService : IService
    {
        public LevelTextData GetLevelTextData(string sceneName, LevelTextsType type);
    }
}
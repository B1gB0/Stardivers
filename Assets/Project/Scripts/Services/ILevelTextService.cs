using Cysharp.Threading.Tasks;
using Project.Scripts.DataBase.Data;
using Project.Scripts.Levels;

namespace Project.Scripts.Services
{
    public interface ILevelTextService
    {
        public UniTask Init();
        public LevelTextData GetLevelTextData(string sceneName, LevelTextsType type);
    }
}
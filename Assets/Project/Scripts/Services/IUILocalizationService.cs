using Project.Scripts.DataBase.Data;
using Project.Scripts.UI;

namespace Project.Scripts.Services
{
    public interface IUILocalizationService : IService
    {
        public UILocalizationData GetLevelTextData(UITextType type);
    }
}
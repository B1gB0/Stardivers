using Cysharp.Threading.Tasks;

namespace Project.Scripts.Services
{
    public interface IResourceService
    {
        UniTask<T> Load<T>(string assetName);
    }
}
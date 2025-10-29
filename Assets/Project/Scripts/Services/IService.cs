using Cysharp.Threading.Tasks;

namespace Project.Scripts.Services
{
    public interface IService
    {
        public bool IsInitiated { get; }
        
        public UniTask Init()
        {
            return UniTask.CompletedTask;
        }
    }
}
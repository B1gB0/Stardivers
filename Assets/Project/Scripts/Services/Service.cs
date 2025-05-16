using Cysharp.Threading.Tasks;

namespace Project.Scripts.Services
{
    public class Service
    {
        public virtual UniTask Init()
        {
            return UniTask.CompletedTask;
        }
    }
}
using Cysharp.Threading.Tasks;

namespace Project.Scripts.UI.View
{
    public interface IView
    {
        public void Show() { }
        public void Hide() { }
        public UniTask HideAsync() { return UniTask.CompletedTask;}
        public UniTask ShowAsync() { return UniTask.CompletedTask;}
    }
}

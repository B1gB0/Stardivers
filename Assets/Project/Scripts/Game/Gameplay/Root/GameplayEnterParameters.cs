using Project.Scripts.Game.GameRoot;

namespace Project.Scripts.Game.Gameplay.Root
{
    public class GameplayEnterParameters : SceneEnterParameters
    {
        public GameplayEnterParameters(int currentNumberLevel, string sceneName) : base(sceneName)
        {
            CurrentNumberLevel = currentNumberLevel;
        }

        public int CurrentNumberLevel { get; }
    }
}
using Project.Scripts.Game.GameRoot;
using Project.Scripts.Levels;

namespace Project.Scripts.Game.Gameplay.Root
{
    public class GameplayEnterParameters : SceneEnterParameters
    {
        public int CurrentNumberLevel { get; }
        
        public GameplayEnterParameters(int currentNumberLevel, string sceneName) : base(sceneName)
        {
            CurrentNumberLevel = currentNumberLevel;
        }
    }
}
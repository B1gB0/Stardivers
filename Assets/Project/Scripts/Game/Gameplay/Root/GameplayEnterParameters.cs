using Project.Scripts.Game.GameRoot;
using Project.Scripts.Levels;

namespace Project.Scripts.Game.Gameplay.Root
{
    public class GameplayEnterParameters : SceneEnterParameters
    {
        public Operation CurrentOperation { get; }
        
        public int CurrentNumberLevel { get; }
        
        public GameplayEnterParameters(Operation currentOperation, 
            int currentNumberLevel) : base(Scenes.Gameplay)
        {
            CurrentOperation = currentOperation;
            CurrentNumberLevel = currentNumberLevel;
        }
    }
}
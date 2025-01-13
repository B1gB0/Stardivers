using Project.Scripts.Game.GameRoot;
using Project.Scripts.Levels;

namespace Project.Scripts.Game.Gameplay.Root
{
    public class GameplayEnterParameters : SceneEnterParameters
    {
        public string SaveFileName { get; }
        
        public Operation CurrentOperation { get; }
        
        public int CurrentNumberLevel { get; }
        
        public GameplayEnterParameters(string saveFileName, Operation currentOperation, 
            int currentNumberLevel) : base(Scenes.Gameplay)
        {
            SaveFileName = saveFileName;
            CurrentOperation = currentOperation;
            CurrentNumberLevel = currentNumberLevel;
        }
    }
}
using Project.Game.Scripts.Game.GameRoot;
using Project.Scripts.Operations;

namespace Build.Game.Scripts.Game.Gameplay
{
    public class GameplayEnterParameters : SceneEnterParameters
    {
        public string SaveFileName { get; }
        
        public string CurrentOperation { get; }
        
        public GameplayEnterParameters( string saveFileName, string currentOperation) : base(Scenes.Gameplay)
        {
            SaveFileName = saveFileName;
            CurrentOperation = currentOperation;
        }
    }
}
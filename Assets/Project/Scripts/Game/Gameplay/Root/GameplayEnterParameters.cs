using Project.Game.Scripts.Game.GameRoot;

namespace Build.Game.Scripts.Game.Gameplay
{
    public class GameplayEnterParameters : SceneEnterParameters
    {
        public string SaveFileName { get; }
        
        public int LevelNumber { get; }
        
        public GameplayEnterParameters( string saveFileName, int levelNumber) : base(Scenes.Gameplay)
        {
            SaveFileName = saveFileName;
            LevelNumber = levelNumber;
        }
    }
}
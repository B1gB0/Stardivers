using Project.Scripts.Game.GameRoot;

namespace Project.Scripts.Game.MainMenu.Root
{
    public class MainMenuEnterParameters : SceneEnterParameters
    {
        public string Result { get; }

        public MainMenuEnterParameters(string result) : base(Scenes.MainMenu)
        {
            Result = result;
        }
    }
}
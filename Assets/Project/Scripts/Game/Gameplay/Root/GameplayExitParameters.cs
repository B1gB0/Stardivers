using Build.Game.Scripts.Game.Gameplay.GameplayRoot;

namespace Build.Game.Scripts.Game.Gameplay
{
    public class GameplayExitParameters
    {
        public MainMenuEnterParameters MainMenuEnterParameters { get; }

        public GameplayExitParameters(MainMenuEnterParameters mainMenuEnterParameters)
        {
            MainMenuEnterParameters = mainMenuEnterParameters;
        }
    }
}
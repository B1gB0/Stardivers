using Project.Scripts.Game.GameRoot;

namespace Project.Scripts.Game.MainMenu.Root
{
    public class MainMenuExitParameters
    {
        public readonly SceneEnterParameters TargetSceneEnterParameters;

        public MainMenuExitParameters(SceneEnterParameters targetSceneEnterParameters)
        {
            TargetSceneEnterParameters = targetSceneEnterParameters;
        }
    }
}
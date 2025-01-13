using Project.Scripts.Game.GameRoot;

namespace Project.Scripts.Game.MainMenu.Root
{
    public class MainMenuExitParameters
    {
        public SceneEnterParameters TargetSceneEnterParameters;

        public MainMenuExitParameters(SceneEnterParameters targetSceneEnterParameters)
        {
            TargetSceneEnterParameters = targetSceneEnterParameters;
        }
    }
}
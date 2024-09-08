using Project.Game.Scripts.Game.GameRoot;

namespace Build.Game.Scripts.Game.Gameplay.GameplayRoot
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
using Project.Scripts.Game.GameRoot;

namespace Project.Scripts.Game.Gameplay.Root
{
    public class GameplayExitParameters
    {
        public SceneEnterParameters TargetSceneEnterParameters;

        public GameplayExitParameters(SceneEnterParameters targetSceneEnterParameters)
        {
            TargetSceneEnterParameters = targetSceneEnterParameters;
        }
    }
}
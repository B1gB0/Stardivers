using Project.Scripts.Game.GameRoot;

namespace Project.Scripts.Game.Gameplay.Root
{
    public class GameplayExitParameters
    {
        public readonly SceneEnterParameters TargetSceneEnterParameters;

        public GameplayExitParameters(SceneEnterParameters targetSceneEnterParameters)
        {
            TargetSceneEnterParameters = targetSceneEnterParameters;
        }
    }
}
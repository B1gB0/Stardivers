namespace Project.Scripts.Game.GameRoot
{
    public abstract class SceneEnterParameters
    {
        public string SceneName { get; private set; }

        public SceneEnterParameters(string sceneName)
        {
            SceneName = sceneName;
        }

        public T As<T>() where T : SceneEnterParameters
        {                                            
            return (T) this;                         
        }

        public void SetNewSceneName(string sceneName)
        {
            SceneName = sceneName;
        }
    }
}
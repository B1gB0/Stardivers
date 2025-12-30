namespace Project.Scripts.Game.GameRoot
{
    public abstract class SceneEnterParameters
    {
        protected SceneEnterParameters(string sceneName)
        {
            SceneName = sceneName;
        }

        public string SceneName { get; private set; }

        public T As<T>() 
            where T : SceneEnterParameters
        {
            return (T) this;
        }

        public void SetNewSceneName(string sceneName)
        {
            SceneName = sceneName;
        }
    }
}
using UnityEngine;

namespace Project.Scripts.UI.View
{
    public abstract class View : MonoBehaviour
    {
        public virtual void Show() { }
        public virtual void Hide() { }
        
        public void Activate()
        {
            gameObject.SetActive(true);
        }

        public void Deactivate()
        {
            gameObject.SetActive(false);
        }
    }
}

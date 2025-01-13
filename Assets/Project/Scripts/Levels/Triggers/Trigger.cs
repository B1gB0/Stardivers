using UnityEngine;

namespace Project.Scripts.Levels.Triggers
{
    public abstract class Trigger : MonoBehaviour
    {
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
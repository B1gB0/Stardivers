using System;
using Project.Scripts.ECS.EntityActors;
using UnityEngine;

namespace Project.Scripts.Levels
{
    public class WelcomeMarsTextTrigger : MonoBehaviour
    {
        public event Action IsWelcomeToMars; 

        private void OnTriggerEnter(Collider trigger)
        {
            if (trigger.TryGetComponent(out PlayerActor _))
            {
                IsWelcomeToMars?.Invoke();
            }
        }

        private void OnTriggerExit(Collider trigger)
        {
            if (trigger.TryGetComponent(out PlayerActor _))
            {
                gameObject.SetActive(false);
            }
        }
    }
}
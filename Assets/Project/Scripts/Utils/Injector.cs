using System;
using Reflex.Extensions;
using Reflex.Injectors;
using UnityEngine;

namespace Source.Game.Scripts.Utils
{
    public class Injector : MonoBehaviour
    {
        public void InjectObject(GameObject targetObject)
        {
            var container = gameObject.scene.GetSceneContainer();
            GameObjectInjector.InjectSingle(targetObject, container);
        }
    }
}
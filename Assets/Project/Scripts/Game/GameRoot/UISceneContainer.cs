﻿using UnityEngine;

namespace Project.Scripts.Game.GameRoot
{
    public class UISceneContainer : MonoBehaviour, IView
    {
        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
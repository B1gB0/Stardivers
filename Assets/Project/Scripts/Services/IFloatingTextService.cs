﻿using UnityEngine;

namespace Project.Scripts.Services
{
    public interface IFloatingTextService
    {
        void OnChangedFloatingText(string value, Transform target, Color targetColor);
    }
}
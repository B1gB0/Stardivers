using System.Collections.Generic;
using Project.Scripts.Projectiles;
using UnityEngine;

namespace Project.Scripts.Lightning
{
    public class LightningProjectile : Projectile
    {
        private const int MinValueLineRenderers = 0;
        private const int MinValuePosition = 2;
        private const int FirstIndex = 0;
        private const int SecondIndex = 1;
        
        [SerializeField] private List<LineRenderer> _lineRenderers = new ();

        public void SetPosition(Vector3 startPoint, Vector3 endPoint)
        {
            if (_lineRenderers.Count > MinValueLineRenderers)
            {
                for (int i = 0; i < _lineRenderers.Count; i++)
                {
                    if (_lineRenderers[i].positionCount >= MinValuePosition)
                    {
                        _lineRenderers[i].SetPosition(FirstIndex, startPoint);
                        _lineRenderers[i].SetPosition(SecondIndex, endPoint);
                    }
                    else
                    {
                        Debug.Log("The line renderer should have at least 2 positions!");
                    }
                }
            }
            else
            {
                Debug.Log("Line Renderers list is empty");
            }
        }
    }
}

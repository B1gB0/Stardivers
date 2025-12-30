using System.Collections.Generic;
using System.Linq;
using Project.Scripts.Projectiles;
using UnityEngine;

namespace Project.Scripts.Lightning
{
    public class LightningLineRendererProjectile : Projectile
    {
        private const int MinValueLineRenderers = 0;
        private const int MinValuePosition = 2;
        private const int FirstIndex = 0;
        private const int SecondIndex = 1;

        [SerializeField] private List<LineRenderer> _lineRenderers = new();

        public void SetPosition(Vector3 startPoint, Vector3 endPoint)
        {
            if (_lineRenderers.Count <= MinValueLineRenderers)
                return;

            foreach (var lineRenderer in _lineRenderers.Where(lineRenderer =>
                         lineRenderer.positionCount >= MinValuePosition))
            {
                lineRenderer.SetPosition(FirstIndex, startPoint);
                lineRenderer.SetPosition(SecondIndex, endPoint);
            }
        }
    }
}
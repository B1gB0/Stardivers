using System.Collections.Generic;
using UnityEngine;

namespace Project.Scripts.Lightning
{
    public class LineRendererController : MonoBehaviour
    {
        private const int MinValueLineRenderers = 0;
        private const int MinValuePosition = 2;
        private const int FirstIndex = 0;
        private const int SecondIndex = 1;
        
        [SerializeField] private List<LineRenderer> _lineRenderers = new ();

        public void SetPosition(Transform startPoint, Transform endPoint)
        {
            if (_lineRenderers.Count > MinValueLineRenderers)
            {
                for (int i = 0; i < _lineRenderers.Count; i++)
                {
                    if (_lineRenderers[i].positionCount >= MinValuePosition)
                    {
                        _lineRenderers[i].SetPosition(FirstIndex, startPoint.position);
                        _lineRenderers[i].SetPosition(SecondIndex, endPoint.position);
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

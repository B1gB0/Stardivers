using System.Collections.Generic;
using UnityEngine;

namespace Project.Scripts.UI.View
{
    public class ItemView : MonoBehaviour, IView
    {
        [SerializeField] private List<Sprite> _sprites;
        
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
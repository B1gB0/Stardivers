using System.Collections.Generic;
using Project.Scripts.UI.View;
using UnityEngine;

namespace Project.Scripts.UI.Panel
{
    public class ShopPanel : MonoBehaviour, IView
    {
        [SerializeField] private List<ItemView> _itemViews;
        
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
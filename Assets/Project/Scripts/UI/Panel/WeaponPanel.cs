using System.Collections.Generic;
using Project.Scripts.UI.View;
using UnityEngine;

namespace Project.Scripts.UI.Panel
{
    public class WeaponPanel : MonoBehaviour, IView
    {
        [SerializeField] private List<WeaponView> _weaponViews;
        
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
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.UI.View
{
    public class WeaponView : MonoBehaviour, IView
    {
        [SerializeField] private Image _filler;
        [SerializeField] private Image _icon;

        public void SetWeaponData()
        {
            
        }
        
        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(true);
        }
    }
}
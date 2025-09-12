using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.UI.View
{
    public class WeaponView : MonoBehaviour, IView
    {
        [SerializeField] private Image _filler;
        [SerializeField] private Image _icon;

        public void SetWeaponData(Sprite sprite)
        {
            _icon.gameObject.SetActive(true);
            _filler.gameObject.SetActive(false);
            _icon.sprite = sprite;
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
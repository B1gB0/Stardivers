using Project.Scripts.Weapon.Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

namespace Project.Scripts.UI.View
{
    public class WeaponView : MonoBehaviour, IView
    {
        private const string MinesKey = "E";
        
        [SerializeField] private Image _filler;
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _text;
        [SerializeField] private TMP_Text _keyText;

        public void SetWeaponData(Sprite sprite, WeaponType type)
        {
            _icon.gameObject.SetActive(true);
            _filler.gameObject.SetActive(false);
            _text.gameObject.SetActive(true);

            if (type == WeaponType.Mines && YG2.envir.isDesktop)
            {
                _keyText.text = MinesKey;
                _keyText.gameObject.SetActive(true);
            }
            
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

        public void ActivateFiller()
        {
            _filler.gameObject.SetActive(true);
            _filler.fillAmount = 1f;
        }

        public void DeactivateFiller()
        {
            _filler.gameObject.SetActive(false);
            _filler.fillAmount = 0f;
        }

        public void SetText(int currentCountShots, int maxShots)
        {
            _text.text = $"{currentCountShots}/{maxShots}";
        }

        public void AnimateFiller(float reloadTimer, float reloadTime)
        {
            float reloadProgress = 1f - (reloadTimer / reloadTime);
            _filler.fillAmount = reloadProgress;
        }
    }
}
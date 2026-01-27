using Project.Scripts.Weapon.Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.UI.View
{
    public class WeaponView : MonoBehaviour
    {
        private const float MinValue = 0f;
        private const float MaxValue = 1f;

        [SerializeField] private Image _filler;
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _text;

        public void SetWeaponData(Sprite sprite, WeaponType type)
        {
            _icon.gameObject.SetActive(true);
            _filler.gameObject.SetActive(false);
            _text.gameObject.SetActive(true);

            _icon.sprite = sprite;
        }

        public void ActivateFiller()
        {
            _filler.gameObject.SetActive(true);
            _filler.fillAmount = MaxValue;
        }

        public void DeactivateFiller()
        {
            _filler.gameObject.SetActive(false);
            _filler.fillAmount = MinValue;
        }

        public void SetText(int currentCountShots, int maxShots)
        {
            _text.text = $"{currentCountShots}/{maxShots}";
        }

        public void AnimateFiller(float reloadTimer, float reloadTime)
        {
            float reloadProgress = MaxValue - (reloadTimer / reloadTime);
            _filler.fillAmount = reloadProgress;
        }
    }
}
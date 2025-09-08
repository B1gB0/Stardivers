using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Project.Scripts.UI.View
{
    public class FloatingTextView : MonoBehaviour, IView
    {
        private const float Delay = 4f;
        private const float Offset = 2f;

        [SerializeField] private List<Sprite> _sprites;
        [SerializeField] private TMP_Text _text;
        [SerializeField] private SpriteRenderer _icon;

        private void OnEnable()
        {
            StartCoroutine(LifeRoutine());
        }

        private void OnDisable()
        {
            StopCoroutine(LifeRoutine());
        }

        public void SetFloatingText(string value, Transform target, FloatingTextViewType viewType, Color color)
        {
            _text.color = color;
            _text.text = $"{value}";
            SetIcon(viewType);
            transform.position = new Vector3 (target.position.x, target.position.y, target.position.z - Offset);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
        
        private void SetIcon(FloatingTextViewType viewType)
        {
            if (viewType == FloatingTextViewType.Gold)
            {
                _icon.sprite = _sprites[0];
            }
            else if(viewType == FloatingTextViewType.RedCrystal)
            {
                _icon.sprite = _sprites[1];
            }
            else
            {
                _icon.sprite = null;
            }
        }

        private IEnumerator LifeRoutine()
        {
            WaitForSeconds waitForSeconds = new WaitForSeconds(Delay);

            yield return waitForSeconds;
            
            Hide();
        }
    }
}
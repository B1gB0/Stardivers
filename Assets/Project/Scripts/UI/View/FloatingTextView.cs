using System.Collections;
using Project.Scripts.UI.View;
using TMPro;
using UnityEngine;

namespace Project.Scripts.UI
{
    public class FloatingTextView : MonoBehaviour, IView
    {
        private const float DamageFactor = 0.9f;
        private const int TextFormat = 10;
        
        private const float Delay = 4f;
        private const float Offset = 2f;
        
        [SerializeField] private TMP_Text _text;

        private void OnEnable()
        {
            StartCoroutine(LifeRoutine());
        }

        private void OnDisable()
        {
            StopCoroutine(LifeRoutine());
        }

        public void SetFloatingText(string value, Transform target, Color targetColor)
        {
            _text.color = targetColor;
            _text.text = $"{value}";
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

        private IEnumerator LifeRoutine()
        {
            WaitForSeconds waitForSeconds = new WaitForSeconds(Delay);

            yield return waitForSeconds;
            
            Hide();
        }
    }
}
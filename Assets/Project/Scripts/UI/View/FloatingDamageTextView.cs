﻿using System.Collections;
using TMPro;
using UnityEngine;

namespace Project.Scripts.UI
{
    public class FloatingDamageTextView : MonoBehaviour, IView
    {
        private const float Delay = 3f;
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

        public void SetDamageText(float damage, Transform target)
        {
            _text.text = damage.ToString();
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
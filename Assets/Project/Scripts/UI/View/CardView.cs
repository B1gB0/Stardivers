using System;
using Project.Scripts.Cards.ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.UI.View
{
    public class CardView : MonoBehaviour, IView
    {
        [SerializeField] private Image _icon;
    
        [SerializeField] private Text _label;
        [SerializeField] private Text _description;
        [SerializeField] private Text _level;
        [SerializeField] private Text _characteristics;

        [SerializeField] private Button _cardViewButton;
    
        private Card _card;

        public event Action<Card, CardView> GetImprovementButtonClicked;

        private void OnEnable()
        {
            SetData();
            _cardViewButton.onClick.AddListener(OnButtonClicked);
        }

        private void OnDisable()
        {
            _cardViewButton.onClick.RemoveListener(OnButtonClicked);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    
        public void GetCard(Card card)
        {
            _card = card;
        }

        private void SetData()
        {
            _icon.sprite = _card.Icon;
            _label.text = _card.Label;
            _description.text = _card.Description;
            _level.text = _card.Level;
        
            if(_card is ImprovementCard improvementCard)
                _characteristics.text = improvementCard.Value * 10 + "% " + improvementCard.CharacteristicType;
            else if (_card is WeaponCard weaponCard)
                _characteristics.text = weaponCard.Characteristics;
        }

        private void OnButtonClicked()
        {
            GetImprovementButtonClicked?.Invoke(_card, this);
        }
    }
}

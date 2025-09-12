using System;
using System.Collections.Generic;
using Project.Scripts.Cards;
using Project.Scripts.Weapon.Player;
using UnityEngine;
using UnityEngine.UI;
using YG;

namespace Project.Scripts.UI.View
{
    public class CardView : MonoBehaviour, IView
    {
        private const int Gun = 0;
        private const int MachineGun = 1;
        private const int FourBarrelMachineGun = 2;
        private const int FragGrenades = 3;
        private const int Mines = 4;
        private const int ChainLightningGun = 5;

        private const string Ru = "ru";
        private const string En = "en";
        private const string Tr = "tr";
        
        [SerializeField] private List<Sprite> _sprites;

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
            _icon.sprite = _card.WeaponType switch
            {
                WeaponType.Gun => _sprites[Gun],
                WeaponType.MachineGun => _sprites[MachineGun],
                WeaponType.FourBarrelMachineGun => _sprites[FourBarrelMachineGun],
                WeaponType.FragGrenades => _sprites[FragGrenades],
                WeaponType.Mines => _sprites[Mines],
                WeaponType.ChainLightningGun => _sprites[ChainLightningGun],
                _ => _icon.sprite
            };

            switch (_card)
            {
                case ImprovementCard improvementCard:
                    _label.text = YG2.lang switch
                    {
                        Ru => improvementCard.CharacteristicsLocalizationData.NameRu,
                        En => improvementCard.CharacteristicsLocalizationData.NameEn,
                        Tr => improvementCard.CharacteristicsLocalizationData.NameTr,
                        _ => _label.text
                    };
                
                    _description.text = YG2.lang switch
                    {
                        Ru => improvementCard.CharacteristicsLocalizationData.DescriptionRu,
                        En => improvementCard.CharacteristicsLocalizationData.DescriptionEn,
                        Tr => improvementCard.CharacteristicsLocalizationData.DescriptionTr,
                        _ => _description.text
                    };
                
                    _level.text = YG2.lang switch
                    {
                        Ru => improvementCard.ImprovementData.LevelCardRu,
                        En => improvementCard.ImprovementData.LevelCardEn,
                        Tr => improvementCard.ImprovementData.LevelCardTr,
                        _ => _level.text
                    };

                    _characteristics.text = _label.text + " " + improvementCard.Value * 10 + "%";
                    break;
                case WeaponCard weaponCard:
                    _label.text = YG2.lang switch
                    {
                        Ru => weaponCard.WeaponLocalizationData.NameRu,
                        En => weaponCard.WeaponLocalizationData.NameEn,
                        Tr => weaponCard.WeaponLocalizationData.NameTr,
                        _ => _label.text
                    };
                
                    _description.text = YG2.lang switch
                    {
                        Ru => weaponCard.WeaponLocalizationData.DescriptionRu,
                        En => weaponCard.WeaponLocalizationData.DescriptionEn,
                        Tr => weaponCard.WeaponLocalizationData.DescriptionTr,
                        _ => _description.text
                    };

                    _level.text = null;
                    _characteristics.text = null;
                    break;
            }
        }

        private void OnButtonClicked()
        {
            GetImprovementButtonClicked?.Invoke(_card, this);
        }
    }
}

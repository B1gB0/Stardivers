using System;
using System.Collections.Generic;
using DG.Tweening;
using Project.Scripts.Cards;
using Project.Scripts.Game.Constant;
using Project.Scripts.Services;
using Project.Scripts.Weapon.CharacteristicsOfWeapon;
using Project.Scripts.Weapon.Player;
using Reflex.Attributes;
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
        private const int Health = 6;
        private const int DiggingSpeed = 7;
        private const int MoveSpeed = 8;

        private const string Common = "Common";
        private const string Unusual = "Unusual";
        private const string Rare = "Rare";
        
        [SerializeField] private int _priceCommonLevel;
        [SerializeField] private int _priceUnusualLevel;
        [SerializeField] private int _priceRareLevel;

        [SerializeField] private GameObject _priceRoot;
        [SerializeField] private Text _priceText;
        
        [SerializeField] private List<Sprite> _sprites;

        [SerializeField] private Image _icon;
    
        [SerializeField] private Text _label;
        [SerializeField] private Text _description;
        [SerializeField] private Text _level;
        [SerializeField] private Text _characteristics;

        [SerializeField] private Button _cardViewButton;

        [SerializeField] private Color _greyColor = Color.gray;
        [SerializeField] private Color _greenColor = Color.green;
        [SerializeField] private Color _blueColor = Color.blue;
    
        private Card _card;
        private ICurrencyService _currencyService;

        public event Action<Card, CardView> GetImprovementButtonClicked;

        [Inject]
        private void Construct(ICurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        private void OnEnable()
        {
            _cardViewButton.onClick.AddListener(OnButtonClicked);
        }

        private void OnDisable()
        {
            _cardViewButton.onClick.RemoveListener(OnButtonClicked);
        }

        private void OnDestroy()
        {
            transform.DOKill();
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void ShowPriceRoot()
        {
            _priceRoot.gameObject.SetActive(true);
        }
        
        public void HidePriceRoot()
        {
            _priceRoot.gameObject.SetActive(false);
        }
    
        public void GetCard(Card card)
        {
            _card = card;
            SetData();
        }

        public void SetData()
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
                    
                    if (_card.WeaponType == WeaponType.None)
                    {
                        _icon.sprite = improvementCard.CharacteristicType switch
                        {
                            CharacteristicType.Health => _sprites[Health],
                            CharacteristicType.DiggingSpeed => _sprites[DiggingSpeed],
                            CharacteristicType.MoveSpeed => _sprites[MoveSpeed],
                            _ => _icon.sprite
                        };
                    }
                    
                    _label.text = YG2.lang switch
                    {
                        LocalizationCode.Ru => improvementCard.CharacteristicsLocalizationData.NameRu,
                        LocalizationCode.En => improvementCard.CharacteristicsLocalizationData.NameEn,
                        LocalizationCode.Tr => improvementCard.CharacteristicsLocalizationData.NameTr,
                        _ => _label.text
                    };
                
                    _description.text = YG2.lang switch
                    {
                        LocalizationCode.Ru => improvementCard.CharacteristicsLocalizationData.DescriptionRu,
                        LocalizationCode.En => improvementCard.CharacteristicsLocalizationData.DescriptionEn,
                        LocalizationCode.Tr => improvementCard.CharacteristicsLocalizationData.DescriptionTr,
                        _ => _description.text
                    };

                    _level.color = improvementCard.ImprovementData.LevelCardEn switch
                    {
                        Common => _greyColor,
                        Unusual => _greenColor,
                        Rare => _blueColor,
                        _ => _level.color
                    };
                    
                    _priceText.text = Convert.ToString(improvementCard.ImprovementData.LevelCardEn switch
                    {
                        Common => _priceCommonLevel,
                        Unusual => _priceUnusualLevel,
                        Rare => _priceRareLevel,
                        _ => _priceText.text
                    });

                    _level.text = YG2.lang switch
                    {
                        LocalizationCode.Ru => improvementCard.ImprovementData.LevelCardRu,
                        LocalizationCode.En => improvementCard.ImprovementData.LevelCardEn,
                        LocalizationCode.Tr => improvementCard.ImprovementData.LevelCardTr,
                        _ => _level.text
                    };

                    if (improvementCard.CharacteristicType is CharacteristicType.MaxCountShots
                        or CharacteristicType.MaxCountEnemiesInChain or CharacteristicType.Health)
                        _characteristics.text = " +" + improvementCard.Value;
                    else
                        _characteristics.text = " +" + improvementCard.Value * 100 + "%";
                    break;
                case WeaponCard weaponCard:
                    _label.text = YG2.lang switch
                    {
                        LocalizationCode.Ru => weaponCard.WeaponLocalizationData.NameRu,
                        LocalizationCode.En => weaponCard.WeaponLocalizationData.NameEn,
                        LocalizationCode.Tr => weaponCard.WeaponLocalizationData.NameTr,
                        _ => _label.text
                    };
                
                    _description.text = YG2.lang switch
                    {
                        LocalizationCode.Ru => weaponCard.WeaponLocalizationData.DescriptionRu,
                        LocalizationCode.En => weaponCard.WeaponLocalizationData.DescriptionEn,
                        LocalizationCode.Tr => weaponCard.WeaponLocalizationData.DescriptionTr,
                        _ => _description.text
                    };

                    _level.text = null;
                    _characteristics.text = null;
                    break;
            }
        }

        private void OnButtonClicked()
        {
            if (_priceRoot.gameObject.activeSelf)
            {
                var price = Convert.ToInt32(_priceText.text);

                if (_currencyService.Gold >= price)
                {
                    _currencyService.SpendGold(price);
                }
                else
                {
                    return;
                }
            }
                
            GetImprovementButtonClicked?.Invoke(_card, this);
        }
    }
}

using System;
using Project.Scripts.Game.Constant;
using Project.Scripts.Levels;
using Project.Scripts.Services;
using Reflex.Attributes;
using UnityEngine;
using UnityEngine.UI;
using YG;

namespace Project.Scripts.UI.View
{
    public class OperationView : MonoBehaviour, IView
    {
        [SerializeField] private Image _image;
        [SerializeField] private Text _name;
        [SerializeField] private Text _description;
        [SerializeField] private Text _countMissions;
        [SerializeField] private GameObject _countMissionsRoot;
        [SerializeField] private Button _startOperation;
        
        [Header("PurchaseUI")]
        [SerializeField] private Text _purchaseOffer;
        [SerializeField] private Text _price;
        [SerializeField] private Button _purchaseButton;
        [SerializeField] private GameObject _lockPanel;

        private Operation _operation;
        private bool _isUnlock;
        private ICurrencyService _currencyService;

        [Inject]
        private void Construct(ICurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        private void OnEnable()
        {
            _purchaseButton.onClick.AddListener(OnPurchaseButtonClicked);
        }

        private void OnDisable()
        {
            _purchaseButton.onClick.RemoveListener(OnPurchaseButtonClicked);
        }

        public void GetOperation(Operation operation)
        {
            _operation = operation;
            SetData();
            CheckAndSetPurchaseState();
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        private void SetData()
        {
            _image.sprite = _operation.Image;

            _name.text = YG2.lang switch
            {
                LocalizationCode.Ru => _operation.NameRu,
                LocalizationCode.En => _operation.NameEn,
                LocalizationCode.Tr => _operation.NameTr,
                _ => _name.text
            };

            _description.text = YG2.lang switch
            {
                LocalizationCode.Ru => _operation.DescriptionRu,
                LocalizationCode.En => _operation.DescriptionEn,
                LocalizationCode.Tr => _operation.DescriptionTr,
                _ => _description.text
            };

            _price.text = _operation.Price.ToString();

            _countMissions.text = _operation.Maps.Count.ToString();
        }
        
        private void CheckAndSetPurchaseState()
        {
            if (CheckUnlockOperation())
            {
                _startOperation.gameObject.SetActive(true);
                _description.gameObject.SetActive(true);
                _countMissionsRoot.gameObject.SetActive(true);
                _purchaseButton.gameObject.SetActive(false);
                _purchaseOffer.gameObject.SetActive(false);
                _lockPanel.gameObject.SetActive(false);
            }
            else
            {
                _startOperation.gameObject.SetActive(false);
                _description.gameObject.SetActive(false);
                _countMissionsRoot.gameObject.SetActive(false);
                _purchaseButton.gameObject.SetActive(true);
                _purchaseOffer.gameObject.SetActive(true);
                _lockPanel.gameObject.SetActive(true);
            }
        }
        
        private bool CheckUnlockOperation()
        {
            switch (_operation.Id)
            {
                case Game.Constant.Operations.Mars:
                {
                    return YG2.saves.isMarsOperationUnlock;
                }
                case Game.Constant.Operations.MysteryPlanet:
                {
                    return YG2.saves.isMysteryPlanetUnlock;
                }
                default:
                    throw new Exception("Operation not found");
            }
        }

        private void OnPurchaseButtonClicked()
        {
            if(_currencyService.Gold < _operation.Price)
                return;
            
            _currencyService.SpendGold(_operation.Price);
            
            switch (_operation.Id)
            {
                case Game.Constant.Operations.MysteryPlanet:
                {
                    YG2.saves.isMysteryPlanetUnlock = true;
                    YG2.SaveProgress();
                    CheckAndSetPurchaseState();
                    break;
                }
                default:
                    throw new Exception("Operation not found");
            }
        }
    }
}
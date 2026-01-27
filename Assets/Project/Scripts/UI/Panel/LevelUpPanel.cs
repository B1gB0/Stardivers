using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Project.Scripts.Audio.Sounds;
using Project.Scripts.Cards;
using Project.Scripts.DataBase.Data;
using Project.Scripts.Game.Constant;
using Project.Scripts.Services;
using Project.Scripts.UI.View;
using Project.Scripts.Weapon.Improvements;
using Project.Scripts.Weapon.Player;
using Reflex.Attributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

namespace Project.Scripts.UI.Panel
{
    public class LevelUpPanel : MonoBehaviour
    {
        private const int MinValue = 0;
        private const float LevelUpDelay = 0.3f;

        private readonly WeaponVisitor _weaponVisitor = new();
        private readonly Queue<int> _pendingLevels = new();

        [SerializeField] private List<CardView> _cardViews = new();

        [SerializeField] private Button _rollButton;
        [SerializeField] private Button _healButton;
        [SerializeField] private Button _continueButton;

        [SerializeField] private TMP_Text _rollPriceTextButton;
        [SerializeField] private TMP_Text _healPriceTextButton;

        [SerializeField] private Text _title;

        [SerializeField] private GameObject _priceRoot;

        [SerializeField] private int _priceOfRoll;
        [SerializeField] private int _priceOfHeal;

        [SerializeField] private float _healthFactor;

        private AudioSoundsService _audioSoundsService;
        private IPauseService _pauseService;
        private IPlayerService _playerService;
        private ICurrencyService _currencyService;
        private ITweenAnimationService _tweenAnimationService;
        private ILevelUpService _levelUpService;
        private IUILocalizationService _uiLocalizationService;

        private WeaponFactory _weaponFactory;
        private WeaponHolder _weaponHolder;
        private WeaponPanel _weaponPanel;
        private HealthBar _healthBar;

        private int _currentLevel;
        private bool _isShowing;
        private bool _isClosed;

        private UILocalizationData _uiLocalizationData;

        public event Action OnContinueButtonIsClicked;

        [Inject]
        private void Construct(
            AudioSoundsService audioSoundsService,
            IPauseService pauseService,
            IPlayerService playerService,
            ICurrencyService currencyService,
            ITweenAnimationService tweenAnimationService,
            ILevelUpService levelUpService,
            IUILocalizationService uiLocalizationService)
        {
            _audioSoundsService = audioSoundsService;
            _pauseService = pauseService;
            _playerService = playerService;
            _currencyService = currencyService;
            _tweenAnimationService = tweenAnimationService;
            _levelUpService = levelUpService;
            _uiLocalizationService = uiLocalizationService;
        }

        private void OnEnable()
        {
            foreach (CardView cardView in _cardViews)
            {
                cardView.GetImprovementButtonClicked += OnCardViewButtonClicked;
            }

            _healButton.onClick.AddListener(OnHealButtonClicked);
            _rollButton.onClick.AddListener(OnRollButtonClicked);
            _continueButton.onClick.AddListener(OnContinueButtonClicked);
        }

        private void Start()
        {
            _currencyService.OnGoldValueChanged += OnChangePriceColorText;

            OnChangePriceColorText(_currencyService.Gold);

            _healPriceTextButton.text = _priceOfHeal.ToString();
            _rollPriceTextButton.text = _priceOfRoll.ToString();
        }

        private void OnDisable()
        {
            foreach (CardView cardView in _cardViews)
            {
                cardView.GetImprovementButtonClicked -= OnCardViewButtonClicked;
            }

            _healButton.onClick.RemoveListener(OnHealButtonClicked);
            _rollButton.onClick.RemoveListener(OnRollButtonClicked);
            _continueButton.onClick.RemoveListener(OnContinueButtonClicked);
        }

        private void OnDestroy()
        {
            _currencyService.OnGoldValueChanged -= OnChangePriceColorText;

            transform.DOKill();
        }

        public void GetServices(
            WeaponFactory weaponFactory,
            WeaponHolder weaponHolder,
            WeaponPanel weaponPanel,
            HealthBar healthBar)
        {
            _weaponFactory = weaponFactory;
            _weaponHolder = weaponHolder;
            _weaponPanel = weaponPanel;
            _healthBar = healthBar;
        }

        public async void OnCurrentLevelIsUpgraded(int currentLevel)
        {
            _healButton.gameObject.SetActive(false);
            _continueButton.gameObject.SetActive(false);

            SetLocalizationData(UITextType.LevelUpPanelTitle);

            HidePriceRoot();

            _pendingLevels.Enqueue(currentLevel);

            if (!_isShowing)
            {
                await ProcessPendingLevels();
            }
        }

        public async void OnEndGameTriggerIsReached()
        {
            _healButton.gameObject.SetActive(true);
            _continueButton.gameObject.SetActive(true);

            SetLocalizationData(UITextType.ShopPanelTitle);

            ShowPriceRoot();

            GetImprovements();
            ShowAndAnimateCardsView();

            await ShowAsync();
        }

        public void OnLanguageChanged()
        {
            if (!gameObject.activeSelf)
                return;

            foreach (var cardView in _cardViews)
            {
                cardView.SetData();
            }
        }

        public void SetTitle()
        {
            if (_uiLocalizationData == null)
                return;

            _title.text = YG2.lang switch
            {
                LocalizationCode.Ru => _uiLocalizationData.NameRu,
                LocalizationCode.En => _uiLocalizationData.NameEn,
                LocalizationCode.Tr => _uiLocalizationData.NameTr,
                _ => _title.text
            };
        }

        private async UniTask ShowAsync()
        {
            if (gameObject.activeSelf)
            {
                await ForceHideAsync();
            }
            
            _healthBar.MoveToWeaponPanelPosition();
            _weaponPanel.Hide();

            await _tweenAnimationService.AnimateScaleAsync(transform);

            _isClosed = false;
        }

        private async UniTask HideAsync()
        {
            if (_isClosed)
                return;

            _isClosed = true;
            _healthBar.MoveToShowPosition();
            _weaponPanel.Show();

            await _tweenAnimationService.AnimateScaleAsync(transform, true);

            await UniTask.NextFrame();
        }

        private void SetLocalizationData(UITextType type)
        {
            _uiLocalizationData = _uiLocalizationService.GetLevelTextData(type);

            SetTitle();
        }

        private void OnChangePriceColorText(int gold)
        {
            _rollPriceTextButton.color =
                Colors.GetColor(gold < _priceOfRoll ? ColorName.RedCurrencyColor : ColorName.DefaultWhiteTextColor);

            _healPriceTextButton.color =
                Colors.GetColor(gold < _priceOfHeal ? ColorName.RedCurrencyColor : ColorName.DefaultWhiteTextColor);
        }

        private void ShowPriceRoot()
        {
            _priceRoot.SetActive(true);
        }

        private void HidePriceRoot()
        {
            _priceRoot.SetActive(false);
        }

        private async UniTask ProcessPendingLevels()
        {
            _isShowing = true;

            while (_pendingLevels.Count > MinValue)
            {
                int level = _pendingLevels.Dequeue();

                await ShowForLevelUp(level);
                await UniTask.WaitUntil(() => _isClosed);
                await UniTask.Delay(TimeSpan.FromSeconds(LevelUpDelay));
            }

            _isShowing = false;
        }

        private async UniTask ShowForLevelUp(int level)
        {
            _currentLevel = level;
            GetCardsForLevelUp(level);

            ShowAndAnimateCardsView();
            await ShowAsync();
        }

        private async UniTask ForceHideAsync()
        {
            _isClosed = true;
            transform.DOKill();
            gameObject.SetActive(false);
            await UniTask.NextFrame();
        }

        private void GetCardsForLevelUp(int currentLevel)
        {
            _levelUpService.GenerateCardsByLevel(currentLevel, _weaponHolder, _cardViews);

            _pauseService.OnStopGameWithoutMusic();
        }

        private void GetImprovements()
        {
            _levelUpService.GenerateImprovements(_cardViews);

            _pauseService.OnStopGameWithoutMusic();
        }

        private async void OnCardViewButtonClicked(Card card, CardView cardView)
        {
            _audioSoundsService.PlaySound(SoundsType.CardViewButton).Forget();

            if (_priceRoot.activeSelf)
            {
                if (_currencyService.Gold >= cardView.Price)
                {
                    _currencyService.SpendGold(cardView.Price);
                    cardView.HidePrice();
                }
                else
                {
                    return;
                }
            }

            if (card is ImprovementCard improvementCard)
            {
                if (improvementCard.WeaponType == WeaponType.None)
                {
                    _playerService.PlayerActor.AcceptImprovement(
                        _weaponVisitor,
                        improvementCard.CharacteristicType,
                        improvementCard.Value);
                }
                else
                {
                    foreach (var weapon in _weaponHolder.Weapons.Where(
                                 weapon => improvementCard.WeaponType == weapon.Type))
                    {
                        weapon.AcceptWeaponImprovement(
                            _weaponVisitor,
                            improvementCard.CharacteristicType,
                            improvementCard.Value);
                    }
                }

                _levelUpService.RemoveImprovementCard(improvementCard);
            }
            else if (card is WeaponCard weaponCard)
            {
                PlayerWeapon weapon = await _weaponFactory.CreateWeapon(weaponCard.WeaponType);
                _levelUpService.UpdateImprovementCardsByTypeWeapon(weapon.Type);
            }

            if (_priceRoot.activeSelf)
            {
                cardView.gameObject.SetActive(false);
            }
            else
            {
                foreach (CardView view in _cardViews)
                {
                    view.Deactivate();
                }

                _pauseService.OnPlayGame();

                await HideAsync();
            }
        }

        private void OnRollButtonClicked()
        {
            if (_currencyService.Gold < _priceOfRoll)
            {
                return;
            }

            _currencyService.SpendGold(_priceOfRoll);

            ShowAndAnimateCardsView();

            if (!_priceRoot.activeSelf)
            {
                _levelUpService.GenerateCardsByLevel(_currentLevel, _weaponHolder, _cardViews);
            }
            else
            {
                foreach (var cardView in _cardViews)
                {
                    cardView.ShowPrice();
                }

                _levelUpService.GenerateImprovements(_cardViews);
            }
        }

        private void OnHealButtonClicked()
        {
            if (_currencyService.Gold < _priceOfHeal)
            {
                return;
            }

            _currencyService.SpendGold(_priceOfHeal);

            _playerService.AddHealthByFactor(_healthFactor);
        }

        private async void OnContinueButtonClicked()
        {
            _pauseService.OnPlayGame();

            await HideAsync();

            OnContinueButtonIsClicked?.Invoke();
        }

        private void ShowAndAnimateCardsView()
        {
            foreach (var cardView in _cardViews)
            {
                cardView.gameObject.SetActive(true);
                _tweenAnimationService.AnimateScale(cardView.transform);
            }
        }
    }
}
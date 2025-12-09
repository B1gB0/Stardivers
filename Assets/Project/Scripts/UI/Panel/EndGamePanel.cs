using System;
using System.Collections.Generic;
using DG.Tweening;
using Project.Scripts.DataBase.Data;
using Project.Scripts.Experience;
using Project.Scripts.Game.Constant;
using Project.Scripts.Services;
using Project.Scripts.UI.View;
using Reflex.Attributes;
using UnityEngine;
using UnityEngine.UI;
using YG;

namespace Project.Scripts.UI.Panel
{
    public class EndGamePanel : MonoBehaviour, IView
    {
        private const int CountCorrectFactor = 1;
        private const string RewardAdRebornId = "RebornPlayer";

        [SerializeField] private Text _labelText;
        [SerializeField] private Text _accumulatedKillsText;
        [SerializeField] private Text _accumulatedGoldText;
        [SerializeField] private Text _accumulatedScoreText;

        [SerializeField] private Button _goToMainMenuButton;
        [SerializeField] private Button _rebornPlayerButton;
        [SerializeField] private Button _nextLevelButton;

        [SerializeField] private List<Image> _images;

        [SerializeField] private GameObject _rootWindow;

        private IPauseService _pauseService;
        private OperationService _operationService;
        private ICurrencyService _currencyService;
        private IUILocalizationService _uiLocalizationService;
        private ITweenAnimationService _tweenAnimationService;
        private ExperiencePoints _experiencePoints;
        private WeaponPanel _weaponPanel;

        private UILocalizationData _uiLocalizationData;

        public event Action OnRewardAdSuccessShowed;

        public Button GoToMainMenuButton => _goToMainMenuButton;
        public Button NextLevelButton => _nextLevelButton;

        [Inject]
        public void Construct(IPauseService pauseService, OperationService operationService,
            ICurrencyService currencyService, IUILocalizationService uiLocalizationService,
            ITweenAnimationService tweenAnimationService)
        {
            _pauseService = pauseService;
            _operationService = operationService;
            _currencyService = currencyService;
            _uiLocalizationService = uiLocalizationService;
            _tweenAnimationService = tweenAnimationService;
        }

        private void OnEnable()
        {
            _goToMainMenuButton.onClick.AddListener(Hide);
            _rebornPlayerButton.onClick.AddListener(OnShowRewardAd);

            _nextLevelButton.onClick.AddListener(OnPlayGame);
            _goToMainMenuButton.onClick.AddListener(OnPlayGame);

#if UNITY_EDITOR
            _rebornPlayerButton.onClick.AddListener(Hide);
            _rebornPlayerButton.onClick.AddListener(OnPlayGame);
            _rebornPlayerButton.onClick.AddListener(OnReborn);
#endif

            YG2.onRewardAdv += OnRewardSuccess;
        }

        private void OnDisable()
        {
            _goToMainMenuButton.onClick.RemoveListener(Hide);
            _rebornPlayerButton.onClick.RemoveListener(OnShowRewardAd);

            _nextLevelButton.onClick.RemoveListener(OnPlayGame);
            _goToMainMenuButton.onClick.RemoveListener(OnPlayGame);

#if UNITY_EDITOR
            _rebornPlayerButton.onClick.RemoveListener(Hide);
            _rebornPlayerButton.onClick.RemoveListener(OnPlayGame);
            _rebornPlayerButton.onClick.RemoveListener(OnReborn);
#endif

            YG2.onRewardAdv -= OnRewardSuccess;
        }

        private void OnDestroy()
        {
            _rootWindow.transform.DOKill();
        }

        public void SetVictoryPanel()
        {
            if (_operationService.CurrentNumberLevel ==
                _operationService.CurrentOperation.Maps.Count - CountCorrectFactor)
            {
                _nextLevelButton.gameObject.SetActive(false);

                if (_operationService.CurrentOperation.Id == Game.Constant.Operations.Mars)
                {
                    YG2.saves.isMysteryPlanetUnlock = true;
                    YG2.SaveProgress();
                }
            }
            else
            {
                _nextLevelButton.gameObject.SetActive(true);
            }

            _rebornPlayerButton.gameObject.SetActive(false);

            SetLocalizationData(UITextType.VictoryPanelTitle);

            OnChangeColor(Colors.GetColor(ColorName.BlueUIPanelColor));
        }

        public void SetDefeatPanel()
        {
            _rebornPlayerButton.gameObject.SetActive(true);
            _nextLevelButton.gameObject.SetActive(false);

            SetLocalizationData(UITextType.DefeatPanelTitle);

            OnChangeColor(Colors.GetColor(ColorName.RedUIPanelColor));
        }

        public void Show()
        {
            _accumulatedGoldText.text = _currencyService.AccumulatedGold.ToString();
            _accumulatedKillsText.text = _experiencePoints.AccumulatedKills.ToString();
            _accumulatedScoreText.text = _experiencePoints.AccumulatedScore.ToString();

            _pauseService.StopGame();
            gameObject.SetActive(true);
            _weaponPanel.Hide();
            _tweenAnimationService.AnimateScale(_rootWindow.transform);
        }

        public void Hide()
        {
            _currencyService.ResetAccumulatedGold();
            _experiencePoints.ResetAccumulatedValues();

            gameObject.SetActive(false);
            _weaponPanel.Show();
        }

        public void SetLabelText()
        {
            if (_uiLocalizationData == null)
                return;

            _labelText.text = YG2.lang switch
            {
                LocalizationCode.Ru => _uiLocalizationData.NameRu,
                LocalizationCode.En => _uiLocalizationData.NameEn,
                LocalizationCode.Tr => _uiLocalizationData.NameTr,
                _ => _labelText.text
            };
        }

        public void GetServices(ExperiencePoints experiencePoints, WeaponPanel weaponPanel)
        {
            _experiencePoints = experiencePoints;
            _weaponPanel = weaponPanel;
        }

        private void SetLocalizationData(UITextType type)
        {
            _uiLocalizationData = _uiLocalizationService.GetLevelTextData(type);

            SetLabelText();
        }

        private void OnChangeColor(Color color)
        {
            foreach (var image in _images)
            {
                image.color = color;
            }
        }

        private void OnPlayGame()
        {
            _pauseService.PlayGame();
        }

#if UNITY_EDITOR
        private void OnReborn()
        {
            OnRewardAdSuccessShowed?.Invoke();
        }
#endif

        private void OnRewardSuccess(string id = null)
        {
            OnRewardAdSuccessShowed?.Invoke();
            Hide();
            _pauseService.PlayGameAndResetAllPauses();
        }

        private void OnShowRewardAd()
        {
            YG2.RewardedAdvShow(RewardAdRebornId);
        }
    }
}
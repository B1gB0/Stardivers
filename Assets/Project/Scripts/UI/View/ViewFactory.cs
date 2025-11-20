using Cysharp.Threading.Tasks;
using Project.Scripts.Experience;
using Project.Scripts.Game.Gameplay.Root.View;
using Project.Scripts.Game.GameRoot;
using Project.Scripts.Services;
using Project.Scripts.UI.Panel;
using Reflex.Attributes;
using Reflex.Core;
using Reflex.Extensions;
using Reflex.Injectors;
using UnityEngine;

namespace Project.Scripts.UI.View
{
    public class ViewFactory : MonoBehaviour
    {
// #if UNITY_EDITOR
        private const string CheatPanelPath = "CheatPanel";
// #endif

        private const string MissionProgressBarPath = "MissionProgressBar";
        private const string HealthBarPath = "HealthBar";
        private const string TextViewPath = "TextView";
        private const string ProgressRadialBarPath = "ProgressRadialBar";
        private const string LevelUpPanelPath = "LevelUpPanel";
        private const string EndGamePanelPath = "EndGamePanel";
        private const string TimerPath = "Timer";
        private const string AdviserMessagePanelPath = "AdviserMessagePanel";
        private const string GoldViewPath = "GoldView";
        private const string AlienCocoonViewPath = "AlienCocoonView";
        private const string ObjectiveTextViewPath = "ObjectiveTextView";

        private IResourceService _resourceService;
        private UIRootView _uiRoot;
        private UIGameplayRootBinder _uiScene;
        private MissionProgressBar _missionProgressBar;
        private ObjectiveTextView _objectiveTextView;
        private AlienCocoonView _alienCocoonView;
        private LevelUpPanel _levelUpPanel;
        private Container _container;

        [Inject]
        public void Construct(IResourceService resourceService)
        {
            _resourceService = resourceService;
        }

        private void OnDestroy()
        {
            if(_missionProgressBar != null)
                _uiRoot.LocalizationLanguageSwitcher.OnLanguageChanged -= _missionProgressBar.SetText;
            if(_objectiveTextView != null)
                _uiRoot.LocalizationLanguageSwitcher.OnLanguageChanged -= _objectiveTextView.SetText;
            if (_levelUpPanel != null)
                _uiRoot.LocalizationLanguageSwitcher.OnLanguageChanged -= _levelUpPanel.OnLanguageChanged;
        }

        public void GetUIRootAndUIScene(UIRootView uiRoot, UIGameplayRootBinder uiScene, Container container)
        {
            _uiRoot = uiRoot;
            _uiScene = uiScene;
            _container = container;
            
            GameObjectInjector.InjectRecursive(_uiScene.gameObject, _container);
        }

        public async UniTask<HealthBar> CreateHealthBar(Health.Health health)
        {
            var healthBarTemplate = await _resourceService.Load<GameObject>(HealthBarPath);
            healthBarTemplate = Instantiate(healthBarTemplate);

            HealthBar healthBar = healthBarTemplate.GetComponent<HealthBar>();
            GameObjectInjector.InjectObject(healthBar.gameObject, _container);
            healthBar.Construct(health);
            healthBar.transform.SetParent(_uiScene.transform);
            healthBar.GetPoints(_uiScene.ShowHealthPoint, _uiScene.HideHealthPoint);
            
            return healthBar;
        }

        public async UniTask<ProgressRadialBar> CreateProgressBar(ExperiencePoints experiencePoints, Transform target)
        {
            var progressRadialBarPlaneTemplate = await _resourceService.Load<GameObject>(ProgressRadialBarPath);
            progressRadialBarPlaneTemplate = Instantiate(progressRadialBarPlaneTemplate);

            ProgressRadialBar progressRadialBar = progressRadialBarPlaneTemplate.GetComponent<ProgressRadialBar>();
            progressRadialBar.Construct(experiencePoints, target);
            return progressRadialBar;
        }

        public async UniTask<FloatingTextView> CreateDamageTextView()
        {
            var textViewTemplate = await _resourceService.Load<GameObject>(TextViewPath);
            textViewTemplate = Instantiate(textViewTemplate);

            FloatingTextView textView = textViewTemplate.GetComponent<FloatingTextView>();
            return textView;
        }

        public async UniTask<LevelUpPanel> CreateLevelUpPanel()
        {
            var levelUpPanelTemplate = await _resourceService.Load<GameObject>(LevelUpPanelPath);
            levelUpPanelTemplate = Instantiate(levelUpPanelTemplate);

            _levelUpPanel = levelUpPanelTemplate.GetComponent<LevelUpPanel>();
            GameObjectInjector.InjectRecursive(_levelUpPanel.gameObject, _container);
            _levelUpPanel.transform.SetParent(_uiScene.transform);
            _uiRoot.LocalizationLanguageSwitcher.OnLanguageChanged += _levelUpPanel.OnLanguageChanged;

            return _levelUpPanel;
        }

        public async UniTask<EndGamePanel> CreateEndGamePanel()
        {
            var endGamePanelTemplate = await _resourceService.Load<GameObject>(EndGamePanelPath);
            endGamePanelTemplate = Instantiate(endGamePanelTemplate);

            EndGamePanel endGamePanel = endGamePanelTemplate.GetComponent<EndGamePanel>();
            GameObjectInjector.InjectObject(endGamePanel.gameObject, _container);
            endGamePanel.transform.SetParent(_uiScene.transform);
            return endGamePanel;
        }

        public async UniTask<Timer> CreateTimer()
        {
            var timerTemplate = await _resourceService.Load<GameObject>(TimerPath);
            timerTemplate = Instantiate(timerTemplate);

            Timer timer = timerTemplate.GetComponent<Timer>();
            GameObjectInjector.InjectObject(timer.gameObject, _container);
            timer.transform.SetParent(_uiScene.transform, false);
            timer.GetPoints(_uiScene.ShowTimerPoint, _uiScene.HideTimerPoint);
            return timer;
        }

        public async UniTask<DialoguePanel> CreateDialoguePanel()
        {
            var adviserMessagePanelTemplate = await _resourceService.Load<GameObject>(AdviserMessagePanelPath);
            adviserMessagePanelTemplate = Instantiate(adviserMessagePanelTemplate);

            DialoguePanel dialoguePanel = adviserMessagePanelTemplate.GetComponent<DialoguePanel>();
            GameObjectInjector.InjectObject(dialoguePanel.gameObject, _container);
            dialoguePanel.transform.SetParent(_uiScene.transform);
            return dialoguePanel;
        }

        public async UniTask<GoldView> CreateGoldView()
        {
            var goldViewTemplate = await _resourceService.Load<GameObject>(GoldViewPath);
            goldViewTemplate = Instantiate(goldViewTemplate);

            GoldView goldView = goldViewTemplate.GetComponent<GoldView>();
            GameObjectInjector.InjectObject(goldView.gameObject, _container);
            goldView.transform.SetParent(_uiScene.transform);
            goldView.GetPoints(_uiScene.ShowGoldPoint, _uiScene.HideGoldPoint);
            
            return goldView;
        }

        public async UniTask<AlienCocoonView> CreateAlienCocoonView()
        {
            var alienCocoonViewTemplate = await _resourceService.Load<GameObject>(AlienCocoonViewPath);
            alienCocoonViewTemplate = Instantiate(alienCocoonViewTemplate);

            _alienCocoonView = alienCocoonViewTemplate.GetComponent<AlienCocoonView>();
            GameObjectInjector.InjectObject(_alienCocoonView.gameObject, _container);
            _alienCocoonView.transform.SetParent(_uiScene.transform, false);
            _alienCocoonView.GetPoints(_uiScene.ShowAlienCocoonPoint, _uiScene.HideAlienCocoonPoint);
            
            return _alienCocoonView;
        }

        public async UniTask<MissionProgressBar> CreateMissionProgressBar()
        {
            var missionBarTemplate = await _resourceService.Load<GameObject>(MissionProgressBarPath);
            missionBarTemplate = Instantiate(missionBarTemplate);

            _missionProgressBar = missionBarTemplate.GetComponent<MissionProgressBar>();
            GameObjectInjector.InjectObject(_missionProgressBar.gameObject, _container);
            _missionProgressBar.transform.SetParent(_uiScene.transform, false);
            _missionProgressBar.GetPoints(_uiScene.ShowMissionProgressPoint, _uiScene.HideMissionProgressPoint);
            _uiRoot.LocalizationLanguageSwitcher.OnLanguageChanged += _missionProgressBar.SetText;
            return _missionProgressBar;
        }
        
        public async UniTask<ObjectiveTextView> CreateObjectiveText()
        {
            var objectiveTextTemplate = await _resourceService.Load<GameObject>(ObjectiveTextViewPath);
            objectiveTextTemplate = Instantiate(objectiveTextTemplate);

            _objectiveTextView = objectiveTextTemplate.GetComponent<ObjectiveTextView>();
            GameObjectInjector.InjectObject(_objectiveTextView.gameObject, _container);
            _objectiveTextView.transform.SetParent(_uiScene.transform, false);
            _objectiveTextView.GetPoints(_uiScene.ShowTimerPoint, _uiScene.HideTimerPoint);
            _uiRoot.LocalizationLanguageSwitcher.OnLanguageChanged += _objectiveTextView.SetText;
            return _objectiveTextView;
        }

// #if UNITY_EDITOR
        public async UniTask<CheatPanel> CreateCheatPanel()
        {
            var cheatPanelTemplate = await _resourceService.Load<GameObject>(CheatPanelPath);
            cheatPanelTemplate = Instantiate(cheatPanelTemplate);

            CheatPanel cheatPanel = cheatPanelTemplate.GetComponent<CheatPanel>();
            GameObjectInjector.InjectObject(cheatPanel.gameObject, _container);
            cheatPanel.transform.SetParent(_uiScene.transform);
            return cheatPanel;
        }
// #endif
    }
}
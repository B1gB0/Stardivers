using Cysharp.Threading.Tasks;
using Project.Scripts.Experience;
using Project.Scripts.Services;
using Project.Scripts.UI.Panel;
using Reflex.Attributes;
using UnityEngine;

namespace Project.Scripts.UI.View
{
    public class ViewFactory : MonoBehaviour
    {
#if UNITY_EDITOR
        private const string CheatPanelPath = "CheatPanel";
#endif

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

        private IResourceService _resourceService;

        [Inject]
        public void Construct(IResourceService resourceService)
        {
            _resourceService = resourceService;
        }

        public async UniTask<HealthBar> CreateHealthBar(Health.Health health)
        {
            var healthBarTemplate = await _resourceService.Load<GameObject>(HealthBarPath);
            healthBarTemplate = Instantiate(healthBarTemplate);

            HealthBar healthBar = healthBarTemplate.GetComponent<HealthBar>();
            healthBar.Construct(health);
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

            LevelUpPanel levelUpPanel = levelUpPanelTemplate.GetComponent<LevelUpPanel>();
            return levelUpPanel;
        }

        public async UniTask<EndGamePanel> CreateEndGamePanel()
        {
            var endGamePanelTemplate = await _resourceService.Load<GameObject>(EndGamePanelPath);
            endGamePanelTemplate = Instantiate(endGamePanelTemplate);

            EndGamePanel endGamePanel = endGamePanelTemplate.GetComponent<EndGamePanel>();
            return endGamePanel;
        }

        public async UniTask<Timer> CreateTimer()
        {
            var timerTemplate = await _resourceService.Load<GameObject>(TimerPath);
            timerTemplate = Instantiate(timerTemplate);

            Timer timer = timerTemplate.GetComponent<Timer>();
            return timer;
        }

        public async UniTask<DialoguePanel> CreateAdviserMessagePanel()
        {
            var adviserMessagePanelTemplate = await _resourceService.Load<GameObject>(AdviserMessagePanelPath);
            adviserMessagePanelTemplate = Instantiate(adviserMessagePanelTemplate);

            DialoguePanel dialoguePanel = adviserMessagePanelTemplate.GetComponent<DialoguePanel>();
            return dialoguePanel;
        }

        public async UniTask<GoldView> CreateGoldView()
        {
            var goldViewTemplate = await _resourceService.Load<GameObject>(GoldViewPath);
            goldViewTemplate = Instantiate(goldViewTemplate);

            GoldView goldView = goldViewTemplate.GetComponent<GoldView>();
            return goldView;
        }

        public async UniTask<AlienCocoonView> CreateAlienCocoonView()
        {
            var alienCocoonViewTemplate = await _resourceService.Load<GameObject>(AlienCocoonViewPath);
            alienCocoonViewTemplate = Instantiate(alienCocoonViewTemplate);

            AlienCocoonView alienCocoonView = alienCocoonViewTemplate.GetComponent<AlienCocoonView>();
            return alienCocoonView;
        }

        public async UniTask<MissionProgressBar> CreateMissionProgressBar()
        {
            var missionBarTemplate = await _resourceService.Load<GameObject>(MissionProgressBarPath);
            missionBarTemplate = Instantiate(missionBarTemplate);

            MissionProgressBar missionProgressBar = missionBarTemplate.GetComponent<MissionProgressBar>();
            return missionProgressBar;
        }

#if UNITY_EDITOR
        public async UniTask<CheatPanel> CreateCheatPanel()
        {
            var cheatPanelTemplate = await _resourceService.Load<GameObject>(CheatPanelPath);
            cheatPanelTemplate = Instantiate(cheatPanelTemplate);

            CheatPanel cheatPanel = cheatPanelTemplate.GetComponent<CheatPanel>();
            return cheatPanel;
        }
#endif
    }
}
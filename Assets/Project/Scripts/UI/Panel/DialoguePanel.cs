using DG.Tweening;
using Project.Scripts.DataBase.Data;
using Project.Scripts.Game.Constant;
using Project.Scripts.Services;
using Project.Scripts.UI.View;
using Reflex.Attributes;
using UnityEngine;
using UnityEngine.UI;
using YG;

namespace Project.Scripts.UI.Panel
{
    [RequireComponent(typeof(Button))]
    public class DialoguePanel : MonoBehaviour, IView
    {
        [SerializeField] private Text _text;

        private IPauseService _pauseService;
        private ITweenAnimationService _tweenAnimationService;
        
        public Button ContinueButton { get; private set; }
        
        [Inject]
        private void Construct(IPauseService pauseService, ITweenAnimationService tweenAnimationService)
        {
            _pauseService = pauseService;
            _tweenAnimationService = tweenAnimationService;
        }

        private void Awake()
        {
            ContinueButton = GetComponent<Button>();
        }

        private void OnEnable()
        {
            ContinueButton.onClick.AddListener(Hide);
        }

        private void OnDisable()
        {
            ContinueButton.onClick.RemoveListener(Hide);
        }

        private void OnDestroy()
        {
            transform.DOKill();
        }

        public void SetText(LevelTextData data)
        {
            _text.text = YG2.lang switch
            {
                LocalizationCode.Ru => data.TextRu,
                LocalizationCode.En => data.TextEn,
                LocalizationCode.Tr => data.TextTr,
                _ => _text.text
            };
        }

        public void Show()
        {
            _pauseService.StopGame();
            gameObject.SetActive(true);
            _tweenAnimationService.AnimateScale(transform);
        }

        public void Hide()
        {
            _pauseService.PlayGame();
            _tweenAnimationService.AnimateScale(transform, true);
        }
    }
}

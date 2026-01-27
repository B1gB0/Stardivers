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
    public class DialoguePanel : MonoBehaviour
    {
        [SerializeField] private Text _text;

        private IPauseService _pauseService;
        private ITweenAnimationService _tweenAnimationService;
        private Button _continueButton;

        [Inject]
        private void Construct(IPauseService pauseService, ITweenAnimationService tweenAnimationService)
        {
            _pauseService = pauseService;
            _tweenAnimationService = tweenAnimationService;
        }

        private void Awake()
        {
            _continueButton = GetComponent<Button>();
        }

        private void OnEnable()
        {
            _continueButton.onClick.AddListener(Hide);
        }

        private void OnDisable()
        {
            _continueButton.onClick.RemoveListener(Hide);
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
            _pauseService.OnStopGameWithoutMusic();
            _tweenAnimationService.AnimateScale(transform);
        }

        public void Hide()
        {
            _pauseService.OnPlayGame();
            _tweenAnimationService.AnimateScale(transform, true);
        }
    }
}
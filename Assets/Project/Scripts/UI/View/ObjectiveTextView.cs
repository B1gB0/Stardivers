using DG.Tweening;
using Project.Scripts.DataBase.Data;
using Project.Scripts.Game.Constant;
using Project.Scripts.Levels;
using Project.Scripts.Services;
using Reflex.Attributes;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using YG;

namespace Project.Scripts.UI.View
{
    public class ObjectiveTextView : MonoBehaviour, IView
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Transform _showPoint;
        [SerializeField] private Transform _hidePoint;

        private ITweenAnimationService _tweenAnimationService;
        private ILevelTextService _levelTextService;
        private LevelTextData _levelTextData;

        [Inject]
        private void Construct(ITweenAnimationService tweenAnimationService, ILevelTextService levelTextService)
        {
            _tweenAnimationService = tweenAnimationService;
            _levelTextService = levelTextService;
        }

        private void Awake()
        {
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            transform.DOKill();
        }

        public void Show()
        {
            SetData();
            gameObject.SetActive(true);
            _tweenAnimationService.AnimateMove(transform, _showPoint, _hidePoint);
        }

        public void Hide()
        {
            _tweenAnimationService.AnimateMove(transform, _showPoint, _hidePoint, true);
        }

        public void GetPoints(Transform showPoint, Transform hidePoint)
        {
            _showPoint = showPoint;
            _hidePoint = hidePoint;
        }

        public void SetData()
        {
            _levelTextData = _levelTextService.GetLevelTextData(
                SceneManager.GetActiveScene().name,
                LevelTextsType.ObjectiveText);

            SetText();
        }

        public void SetText()
        {
            if (_levelTextData == null)
                return;

            _text.text = YG2.lang switch
            {
                LocalizationCode.Ru => _levelTextData.TextRu,
                LocalizationCode.En => _levelTextData.TextEn,
                LocalizationCode.Tr => _levelTextData.TextTr,
                _ => _text.text
            };
        }
    }
}
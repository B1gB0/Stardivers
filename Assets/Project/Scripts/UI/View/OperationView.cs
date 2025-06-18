using Project.Scripts.Levels;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.UI;
using YG;

namespace Project.Scripts.UI.View
{
    public class OperationView : MonoBehaviour, IView
    {
        private const string Ru = "ru";
        private const string En = "en";
        private const string Tr = "tr";

        [SerializeField] private Image _image;
        [SerializeField] private Text _name;
        [SerializeField] private Text _countMissions;

        private Operation _operation;

        public void GetOperation(Operation operation)
        {
            _operation = operation;
            SetData();
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
                Ru => _operation.NameRu,
                En => _operation.NameEn,
                Tr => _operation.NameTr,
                _ => _name.text
            };

            _countMissions.text = _operation.Maps.Count.ToString();
        }
    }
}
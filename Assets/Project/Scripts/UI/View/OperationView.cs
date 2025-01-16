using Project.Scripts.Levels;
using UnityEngine;
using UnityEngine.UI;
using YG;

namespace Project.Scripts.UI.View
{
    public class OperationView : MonoBehaviour, IView
    {
        private const string CountMissionsText = "The number of missions in the operation: ";
        
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
            _name.text = _operation.Name;
            _countMissions.text = CountMissionsText + _operation.Maps.Count;
        }
    }
}
using Project.Scripts.Levels;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.UI;
using YG;

namespace Project.Scripts.UI.View
{
    public class OperationView : MonoBehaviour, IView
    {
        private const int CountLanguages = 2;
        
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
            Profiler.BeginSample("TrashView");
            _image.sprite = _operation.Image;
            _name.text = _operation.Name;
            _countMissions.text = _operation.Maps.Count.ToString();
            Profiler.EndSample();
        }
    }
}
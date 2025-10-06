using Project.Scripts.Services;
using Reflex.Attributes;
using TMPro;
using UnityEngine;

namespace Project.Scripts.UI.View
{
    public class GoldView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;

        private IGoldService _goldService;

        [Inject]
        private void Construct(IGoldService goldService)
        {
            _goldService = goldService;
            _text.text = _goldService.Gold.ToString();
            _goldService.OnValueChanged += SetValue;
        }

        private void SetValue(int value)
        {
            _text.text = _goldService.Gold.ToString();
        }

        private void OnDestroy()
        {
            _goldService.OnValueChanged -= SetValue;
        }
    }
}

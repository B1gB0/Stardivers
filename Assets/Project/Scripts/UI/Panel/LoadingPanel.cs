using TMPro;
using UnityEngine;

namespace Project.Scripts.UI.Panel
{
    public class LoadingPanel : View.View
    {
        private const int TextFormat = 100;
        private const float StartProgress = 0f;

        [SerializeField] private TMP_Text _progressText;

        public override void Show()
        {
            Activate();
            _progressText.text = $"{StartProgress * TextFormat:0}%";
        }

        public override void Hide()
        {
            Deactivate();
        }

        public void SetProgressText(float progress)
        {
            _progressText.text = $"{progress * TextFormat:0}%";
        }
    }
}
using DG.Tweening;
using UnityEngine;

namespace Project.Scripts.UI.View
{
    public class JoystickView : View
    {
        private void OnDestroy()
        {
            transform.DOKill();
        }
    }
}

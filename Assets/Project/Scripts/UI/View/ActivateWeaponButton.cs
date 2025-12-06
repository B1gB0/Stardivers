using UnityEngine;
using UnityEngine.UI;
using YG;

namespace Project.Scripts.UI.View
{
    public class ActivateWeaponButton : MonoBehaviour
    {
        [SerializeField] private Image _keyIcon;
        
        private void Start()
        {
            if (YG2.envir.isDesktop)
            {
                _keyIcon.gameObject.SetActive(true);
            }
        }
    }
}
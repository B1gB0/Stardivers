using TMPro;
using UnityEngine;
using YG;

namespace Project.Scripts.UI.View
{
    public class ActivateWeaponButton : MonoBehaviour
    {
        private const string MinesKey = "E";
        
        [SerializeField] private TMP_Text _keyText;
        
        private void Start()
        {
            if (YG2.envir.isDesktop)
            {
                _keyText.text = MinesKey;
                _keyText.gameObject.SetActive(true);
            }
        }
    }
}
using Project.Scripts.UI.View;
using UnityEngine;

public class Joystick : MonoBehaviour, IView
{
    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}

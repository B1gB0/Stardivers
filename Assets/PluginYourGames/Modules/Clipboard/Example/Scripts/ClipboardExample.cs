using UnityEngine;

namespace YG.Example
{
    public class ClipboardExample : MonoBehaviour
    {
        void Update()
        {
            if (Input.GetKey(KeyCode.LeftControl))
            {
                if (Input.GetKeyUp(KeyCode.C))
                {
                    YG2.SetClipboardText("Hello!!!");
                }

                if (Input.GetKeyUp(KeyCode.V))
                {
                    YG2.GetClipboardTextAsync(clipboardText =>
                    {
                        if (clipboardText != null)
                        {
                            Debug.Log("����� �� ������ ������: " + clipboardText);
                        }
                        else
                        {
                            Debug.LogError("����� ������ ���� ��� ��������� ������");
                        }
                    });
                }
            }
        }
    }
}
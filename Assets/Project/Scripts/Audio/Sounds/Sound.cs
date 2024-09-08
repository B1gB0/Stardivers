using System.Collections;
using UnityEngine;

public abstract class Sound : MonoBehaviour
{
    [field: SerializeField] public AudioSource AudioSource { get; private set; }

    public IEnumerator OffSound()
    {
        yield return new WaitForSeconds(AudioSource.clip.length);
            
        gameObject.SetActive(false);
    }
}

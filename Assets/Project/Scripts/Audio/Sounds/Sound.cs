using System.Collections;
using UnityEngine;

public abstract class Sound : MonoBehaviour
{
    [field: SerializeField] public AudioSource AudioSource { get; private set; }

    public IEnumerator OffSoundAfterPlay()
    {
        yield return new WaitForSeconds(AudioSource.clip.length);
            
        gameObject.SetActive(false);
    }
    
    public IEnumerator OffSoundAfterPlay(float playTime)
    {
        yield return new WaitForSeconds(playTime);
            
        gameObject.SetActive(false);
    }
}

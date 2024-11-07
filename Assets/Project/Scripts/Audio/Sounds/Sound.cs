using System.Collections;
using UnityEngine;

public abstract class Sound : MonoBehaviour
{
    [field: SerializeField] public AudioSource AudioSource { get; private set; }

    public IEnumerator OffPoolSoundAfterPlay()
    {
        yield return new WaitForSeconds(AudioSource.clip.length);
            
        gameObject.SetActive(false);
    }
    
    public IEnumerator OffSoundAfterDurationPlay(float playTime)
    {
        AudioSource.PlayOneShot(AudioSource.clip);
        
        yield return new WaitForSeconds(playTime);

        AudioSource.Stop();
    }
}

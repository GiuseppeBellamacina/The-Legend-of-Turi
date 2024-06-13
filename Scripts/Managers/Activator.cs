using UnityEngine;

public class Activator : MonoBehaviour
{
    public GameObject objectToActivate;
    
    public void Enable()
    {
        if (!AudioManager.Instance.sfxSource.isPlaying)
            AudioManager.Instance.PlaySFX(AudioManager.Instance.context);
        objectToActivate.SetActive(true);
    }

    public void Disable()
    {
        objectToActivate.SetActive(false);
    }
}
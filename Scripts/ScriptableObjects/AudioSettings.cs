using UnityEngine;

[CreateAssetMenu]
public class AudioSettings : ScriptableObject, IResettable
{
    public float maxVolume;
    public float minVolume;
    public float defaultVolume;
    public float currentMasterVolume;
    public float currentMusicVolume;
    public float currentSFXVolume;
    public bool mute;

    public void Reset()
    {
        currentMasterVolume = defaultVolume;
        currentMusicVolume = defaultVolume;
        currentSFXVolume = defaultVolume;
        mute = false;
    }
}
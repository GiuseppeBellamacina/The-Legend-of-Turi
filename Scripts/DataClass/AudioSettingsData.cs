[System.Serializable]
public class AudioSettingsData : DataClass
{
    public float maxVolume;
    public float minVolume;
    public float defaultVolume;
    public float currentMasterVolume;
    public float currentMusicVolume;
    public float currentSFXVolume;
    public bool mute;

    public AudioSettingsData(AudioSettings data) : base(data)
    {
        maxVolume = data.maxVolume;
        minVolume = data.minVolume;
        defaultVolume = data.defaultVolume;
        currentMasterVolume = data.currentMasterVolume;
        currentMusicVolume = data.currentMusicVolume;
        currentSFXVolume = data.currentSFXVolume;
        mute = data.mute;
    }
}
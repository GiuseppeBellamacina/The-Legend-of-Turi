using System.IO;
using UnityEngine;

[CreateAssetMenu]
public class AudioSettings : Data
{
    public float maxVolume;
    public float minVolume;
    public float defaultVolume;
    public float currentMasterVolume;
    public float currentMusicVolume;
    public float currentSFXVolume;
    public bool mute;

    public new void Reset()
    {
        currentMasterVolume = defaultVolume;
        currentMusicVolume = defaultVolume;
        currentSFXVolume = defaultVolume;
        mute = false;
    }

    public new void Save()
    {
        dataIndex = 0; // 0 è l'indice solo per l'audio
        string path = dataIndex.ToString() + ".save";
        fileName = name;
        AudioSettingsData data = new AudioSettingsData(this);
        SaveSystem.Save(data, path);
    }

    public new void Load(int index)
    {
        string path = index.ToString() + ".save";
        
        // Se il file non esiste allora ne creo uno nuovo
        if (!File.Exists(SaveSystem.path + path))
        {
            Reset();
            Save();
            return;
        }

        AudioSettingsData data = SaveSystem.Load<AudioSettingsData>(path);
        if (data != null)
        {
            dataIndex = data.dataIndex;
            fileName = data.fileName;
            maxVolume = data.maxVolume;
            minVolume = data.minVolume;
            defaultVolume = data.defaultVolume;
            currentMasterVolume = data.currentMasterVolume;
            currentMusicVolume = data.currentMusicVolume;
            currentSFXVolume = data.currentSFXVolume;
            mute = data.mute;
        }
    }
}
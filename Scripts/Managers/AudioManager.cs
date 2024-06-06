using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
    public AudioMixer audioMixer;
    public AudioSettings data;
    public bool onValueChange;

    public static AudioManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<AudioManager>();
                if (_instance == null)
                    Debug.LogError("No AudioManager found in the scene.");
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    void Start()
    {
        //data.Reset();
        if(data.mute)
        {
            MuteVolume();
        }
        else
        {
            UnMuteVolume();
        }
        onValueChange = true; // serve per evitare che vengano eliminati i dati salvati se metto il volume a 0
    }

    public IEnumerator FadeVolumeCo(float duration, float targetVolume = -80)
    {
        audioMixer.GetFloat("Master", out float startVolume);
        float endVolume = targetVolume;
        float currentTime = 0;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            float newVolume = Mathf.Lerp(startVolume, endVolume, currentTime / duration);
            audioMixer.SetFloat("Master", newVolume);
            yield return null;
        }
    }

    public void FadeVolume(float duration, float targetVolume = -80)
    {
        StartCoroutine(FadeVolumeCo(duration, targetVolume));
    }

    public float PercentizeVolume(float value)
    {
        if (value < data.minVolume)
            value = data.minVolume;

        float tmp = (value + Mathf.Abs(data.minVolume)) / (data.maxVolume - data.minVolume);
        return tmp * 100;
    }

    public float ConvertToVolume(float value)
    {
        if (value <= 0)
            return -80;

        if (value > 100)
            value = 100;

        float tmp = (value / 100) * (data.maxVolume - data.minVolume);
        return tmp + data.minVolume;
    }

    public void SetVolume(string parameter, float inValue)
    {
        // Posso inserire il volume come un valore da 0 a 100
        float value = ConvertToVolume(inValue);
        audioMixer.SetFloat(parameter, value);

        if (!onValueChange)
            return;

        switch (parameter)
        {
            case "Master":
                data.currentMasterVolume = value;
                break;
            case "Music":
                data.currentMusicVolume = value;
                break;
            case "SFX":
                data.currentSFXVolume = value;
                break;
        }
    }

    public void MuteOrUnMuterVolume()
    {
        if (data.mute)
            UnMuteVolume();
        else
            MuteVolume();
    }

    void MuteVolume()
    {
        // Setto i volumi a -80
        float value = -80;
        
        audioMixer.SetFloat("Master", value);
        audioMixer.SetFloat("Music", value);
        audioMixer.SetFloat("SFX", value);

        data.mute = true;
    }

    void UnMuteVolume()
    {
        audioMixer.SetFloat("Master", data.currentMasterVolume);
        audioMixer.SetFloat("Music", data.currentMusicVolume);
        audioMixer.SetFloat("SFX", data.currentSFXVolume);

        data.mute = false;
    }
}
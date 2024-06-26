using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Manager Settings")]
    private static AudioManager _instance;
    public AudioMixer audioMixer;
    public AudioSettings data;
    public bool onValueChange;
    public AudioSource musicSource;
    public AudioSource sfxSource;
    public bool decreased;
    [Header("Interaction Clips")]
    public AudioClip interactionStart;
    public AudioClip continueInteraction;
    public AudioClip interactionEnd;
    public AudioClip context;

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
        
        data.Load(0); // 0 è l'indice solo per l'audio
    }

    void Start()
    {
        if(data.mute)
        {
            MuteVolume();
        }
        else
        {
            UnMuteVolume();
        }

        onValueChange = true; // serve per evitare che vengano eliminati i dati salvati se metto il volume a 0

        AudioSource[] sources = GetComponents<AudioSource>();

        musicSource = sources[0];
        musicSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Music")[0];

        sfxSource = sources[1];
        sfxSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("SFX")[0];
    }

    public IEnumerator FadeVolumeCo(float duration, string parameter, float targetVolume = -80)
    {
        audioMixer.GetFloat(parameter, out float startVolume);
        float endVolume = targetVolume;
        float currentTime = 0;
        while (currentTime < duration)
        {
            currentTime += Time.unscaledDeltaTime;
            float newVolume = Mathf.Lerp(startVolume, endVolume, currentTime / duration);
            audioMixer.SetFloat(parameter, newVolume);
            yield return null;
        }
    }

    public void FadeVolume(float duration, string parameter = "Master", float targetVolume = -80)
    {
        StartCoroutine(FadeVolumeCo(duration, parameter, targetVolume));
    }

    public void DecreaseMusic(float time)
    {
        if (decreased)
            return;

        float volume = PercentizeVolume(data.currentMusicVolume);
        volume *= 0.75f;
        volume = ConvertToVolume(volume);

        FadeVolume(time, "Music", volume);
        decreased = true;
    }

    public void IncreaseMusic(float time)
    {
        if (!decreased)
            return;

        FadeVolume(time, "Music", data.currentMusicVolume);
        decreased = false;
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
        
        data.Save();
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
        data.Save();
    }

    void UnMuteVolume()
    {
        audioMixer.SetFloat("Master", data.currentMasterVolume);
        audioMixer.SetFloat("Music", data.currentMusicVolume);
        audioMixer.SetFloat("SFX", data.currentSFXVolume);

        data.mute = false;
        data.Save();
    }

    public void PlaySFX(AudioClip clip)
    {
        if (sfxSource.isPlaying)
            sfxSource.Stop();
        sfxSource.PlayOneShot(clip);
    }

    public void PlayRandomSFX(AudioClip[] clips)
    {
        if (sfxSource.isPlaying)
            sfxSource.Stop();
        sfxSource.PlayOneShot(clips[Random.Range(0, clips.Length)]);
    }
}
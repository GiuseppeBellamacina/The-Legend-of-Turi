using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
    public AudioMixer audioMixer;

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
        
        if (PlayerPrefs.HasKey("Master")){
            audioMixer.SetFloat("Master", PlayerPrefs.GetFloat("Master"));
        }
        else{
            audioMixer.SetFloat("Master", -30);
            PlayerPrefs.SetFloat("Master", -30);
        }
        if (PlayerPrefs.HasKey("Music")){
            audioMixer.SetFloat("Music", PlayerPrefs.GetFloat("Music"));
        }
        else{
            audioMixer.SetFloat("Music", -30);
            PlayerPrefs.SetFloat("Music", -30);
        }
        if (PlayerPrefs.HasKey("SFX")){
            audioMixer.SetFloat("SFX", PlayerPrefs.GetFloat("SFX"));
        }
        else{
            audioMixer.SetFloat("SFX", -30);
            PlayerPrefs.SetFloat("SFX", -30);
        }
    }
}
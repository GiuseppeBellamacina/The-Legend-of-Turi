using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    public Toggle fullscreenToggle, vsyncToggle;
    public TMP_Text resLabel;
    public TMP_Text volumeLabel, musicLabel, sfxLabel;
    public Slider volumeSlider, musicSlider, sfxSlider;
    int selectedResIndex;
    List<ResItem> resolutions = new List<ResItem>
    {
        new ResItem { width = 800, height = 600 },
        new ResItem { width = 1024, height = 768 },
        new ResItem { width = 1280, height = 720 },
        new ResItem { width = 1920, height = 1080 },
        new ResItem { width = 2560, height = 1440 },
        new ResItem { width = 3840, height = 2160 }
    };

    void Start(){
        fullscreenToggle.isOn = Screen.fullScreen;
        vsyncToggle.isOn = QualitySettings.vSyncCount == 1;

        bool foundRes = false;

        for (int i = 0; i < resolutions.Count; i++){
            if (Screen.width == resolutions[i].width && Screen.height == resolutions[i].height){   
                foundRes = true;
                selectedResIndex = i;
                UpdateResLabel();
                break;
            }
        }

        if (!foundRes){
            resLabel.text = Screen.width.ToString() + " x " + Screen.height.ToString();
            ResItem customRes = new ResItem { width = Screen.width, height = Screen.height };
            resolutions.Add(customRes);
            resolutions.Sort((a, b) => a.width * a.height - b.width * b.height);
            selectedResIndex = resolutions.IndexOf(customRes);
            UpdateResLabel();
        }

        // Qui setto i valori dei volumi in base a quelli salvati in PlayerPrefs
        float vol = 0f;
        vol = PlayerPrefs.GetFloat("MasterVol");
        vol = vol == 0 ? -25 : vol;
        volumeSlider.value = vol;
        volumeLabel.text = InterpolateVolume(vol);

        vol = PlayerPrefs.GetFloat("MusicVol");
        vol = vol == 0 ? -25 : vol;
        musicSlider.value = vol;
        musicLabel.text = InterpolateVolume(vol);

        vol = PlayerPrefs.GetFloat("SFXVol");
        vol = vol == 0 ? -25 : vol;
        sfxSlider.value = vol;
        sfxLabel.text = InterpolateVolume(vol);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(GameObject.Find("Fullscreen Toggle"));
    }

    string InterpolateVolume(float value){
        if (value < -25){
            value = -25;
        }
        float tmp = (value + 25) / 45;
        return (tmp * 100).ToString("0");
    }

    public void SetResolution(){
        ResItem res = resolutions[selectedResIndex];
        Screen.SetResolution(res.width, res.height, fullscreenToggle.isOn);
    }

    public void UpdateResLabel(){
        resLabel.text = resolutions[selectedResIndex].width.ToString() + " x " + resolutions[selectedResIndex].height.ToString();
    }

    public void ResLeft(){
        if (selectedResIndex > 0){
            selectedResIndex--;
        }
        UpdateResLabel();
    }

    public void ResRight(){
        if (selectedResIndex < resolutions.Count - 1){
            selectedResIndex++;
        }
        UpdateResLabel();
    }

    public void ApplySettings(){
        Screen.fullScreen = fullscreenToggle.isOn;
        QualitySettings.vSyncCount = vsyncToggle.isOn ? 1 : 0;
        SetResolution();
    }

    public void SetMasterVolume(){   
        volumeLabel.text = InterpolateVolume(volumeSlider.value);
        float to_set = volumeSlider.value == -25 ? -80 : volumeSlider.value;
        AudioManager.Instance.audioMixer.SetFloat("Master", to_set);
        PlayerPrefs.SetFloat("Master", to_set);
    }

    public void SetMusicVolume(){
        musicLabel.text = InterpolateVolume(musicSlider.value);
        float to_set = musicSlider.value == -25 ? -80 : musicSlider.value;
        AudioManager.Instance.audioMixer.SetFloat("Music", to_set);
        PlayerPrefs.SetFloat("Music", to_set);
    }

    public void SetSFXVolume(){
        sfxLabel.text = InterpolateVolume(sfxSlider.value);
        float to_set = sfxSlider.value == -25 ? -80 : sfxSlider.value;
        AudioManager.Instance.audioMixer.SetFloat("SFX", to_set);
        PlayerPrefs.SetFloat("SFX", to_set);
    }
}

[System.Serializable]
public class ResItem
{
    public int width, height;
}
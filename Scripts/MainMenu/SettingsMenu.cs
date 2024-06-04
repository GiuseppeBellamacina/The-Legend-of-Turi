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

    void Start(){
        fullscreenToggle.isOn = Screen.fullScreen;
        vsyncToggle.isOn = QualitySettings.vSyncCount == 1;

        bool foundRes = false;

        List<ResItem> resolutions = ScreenManager.Instance.resolutions;
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
            ScreenManager.Instance.resolutions = resolutions;
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
        EventSystem.current.SetSelectedGameObject(fullscreenToggle.gameObject);
    }

    string InterpolateVolume(float value){
        if (value < -25){
            value = -25;
        }
        float tmp = (value + 25) / 45;
        return (tmp * 100).ToString("0");
    }

    public void UpdateResLabel(){
        List<ResItem> resolutions = ScreenManager.Instance.resolutions;
        resLabel.text = resolutions[selectedResIndex].width.ToString() + " x " + resolutions[selectedResIndex].height.ToString();
    }

    public void ResLeft(){
        if (selectedResIndex > 0){
            selectedResIndex--;
        }
        UpdateResLabel();
    }

    public void ResRight(){
        List<ResItem> resolutions = ScreenManager.Instance.resolutions;
        if (selectedResIndex < resolutions.Count - 1){
            selectedResIndex++;
        }
        UpdateResLabel();
    }

    public void ApplySettings(){
        bool fullScreen = fullscreenToggle.isOn;
        bool vsync = vsyncToggle.isOn;
        ScreenManager.Instance.SetResolution(selectedResIndex, fullScreen, vsync);
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
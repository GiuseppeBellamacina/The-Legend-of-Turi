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
    public GameObject muteButton;
    public Sprite muteSprite, unmuteSprite;
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

        AudioManager.Instance.onValueChange = false;

        if (AudioManager.Instance.data.mute)
        {
            volumeSlider.value = 0;
            musicSlider.value = 0;
            sfxSlider.value = 0;
            
            volumeLabel.text = "0";
            musicLabel.text = "0";
            sfxLabel.text = "0";

            muteButton.GetComponent<Image>().sprite = unmuteSprite;
        }   
        else
        {
            volumeSlider.value = AudioManager.Instance.PercentizeVolume(AudioManager.Instance.data.currentMasterVolume);
            musicSlider.value = AudioManager.Instance.PercentizeVolume(AudioManager.Instance.data.currentMusicVolume);
            sfxSlider.value = AudioManager.Instance.PercentizeVolume(AudioManager.Instance.data.currentSFXVolume);

            volumeLabel.text = volumeSlider.value.ToString("0");
            musicLabel.text = musicSlider.value.ToString("0");
            sfxLabel.text = sfxSlider.value.ToString("0");

            muteButton.GetComponent<Image>().sprite = muteSprite;
        }

        AudioManager.Instance.onValueChange = true;
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
        if (!AudioManager.Instance.onValueChange)
            return;

        float volume = volumeSlider.value;
        if (volume == 0 || (AudioManager.Instance.data.mute && volume >= 0))
        {
            Mute();
            AudioManager.Instance.onValueChange = false;
            volumeSlider.value = volume;
            AudioManager.Instance.onValueChange = true;
        }
        volumeLabel.text = volume.ToString("0");
        AudioManager.Instance.SetVolume("Master", volume);
    }

    public void SetMusicVolume(){
        if (!AudioManager.Instance.onValueChange)
            return;

        float volume = musicSlider.value;
        musicLabel.text = volume.ToString("0");
        AudioManager.Instance.SetVolume("Music", volume);
    }

    public void SetSFXVolume(){
        if (!AudioManager.Instance.onValueChange)
            return;

        float volume = sfxSlider.value;
        sfxLabel.text = volume.ToString("0");
        AudioManager.Instance.SetVolume("SFX", volume);
    }

    public void Mute()
    {
        AudioManager.Instance.onValueChange = false;
        AudioManager.Instance.MuteOrUnMuterVolume();

        if (!AudioManager.Instance.data.mute)
        {
            volumeSlider.value = AudioManager.Instance.PercentizeVolume(AudioManager.Instance.data.currentMasterVolume);
            musicSlider.value = AudioManager.Instance.PercentizeVolume(AudioManager.Instance.data.currentMusicVolume);
            sfxSlider.value = AudioManager.Instance.PercentizeVolume(AudioManager.Instance.data.currentSFXVolume);
            volumeLabel.text = volumeSlider.value.ToString("0");
            musicLabel.text = musicSlider.value.ToString("0");
            sfxLabel.text = sfxSlider.value.ToString("0");
            muteButton.GetComponent<Image>().sprite = muteSprite;
        }
        else
        {
            volumeSlider.value = 0;
            musicSlider.value = 0;
            sfxSlider.value = 0;
            volumeLabel.text = "0";
            musicLabel.text = "0";
            sfxLabel.text = "0";
            muteButton.GetComponent<Image>().sprite = unmuteSprite;
        }
        AudioManager.Instance.onValueChange = true;
    }
}
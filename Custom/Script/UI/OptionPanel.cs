using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionPanel : MonoBehaviour
{
    public Dropdown resolution;
    public Dropdown language;

    [Space]
    public Scrollbar brightness;
    public Scrollbar bgm;
    public Scrollbar sfx;
    public Scrollbar ui;

    [Space]
    public Toggle mute;
    public Toggle fullscreen;

    private void OnEnable() {
        switch(ResolutionManager.windowedWidth)
        {
            case 1024 :
                resolution.value = 0;
                break;
            case 1152 :
                resolution.value = 1;
                break;
            case 1280 :
                resolution.value = 2;
                break;
            case 1366 :
                resolution.value = 3;
                break;
            case 1600 :
                resolution.value = 4;
                break;
            case 1920 :
                resolution.value = 5;
                break;
            case 2560 :
                resolution.value = 6;
                break;
        }

        switch(LanguageManager.currentLanguage)
        {
            case "Korean" :
                language.value = 0;
                break;
            case "English" :
                language.value = 1;
                break;
        }

        brightness.value = BrightnessManager.brightness;

        bgm.value = GameAudioManager.currentBackGroundVolume;
        sfx.value = GameAudioManager.currentSFXVolume;
        ui.value = GameAudioManager.currentUIVolume;

        mute.isOn = !(GameAudioManager.totalVolme > 0);
        fullscreen.isOn = ResolutionManager.isFullScreen;
        
    }


    public void setFullScreen(bool isOn)
    {
        ResolutionManager.isFullScreen = isOn;
        ResolutionManager.SetFullScreen(ResolutionManager.isFullScreen);
    }

    public void ResolutionChange(Dropdown dropdown)
    {
        int value = dropdown.value;
        ResolutionManager.SetResolution(value);
    }
    public void LanguageChange(Dropdown dropdown)
    {
        string[] strings = {"Korean", "English"};
        int value = dropdown.value;
        LanguageManager.Invoke_languageChangeEvent(strings[value]);
    }

    public void BrightnessChange(float value)
    {
        BrightnessManager.instance.setBrightNess(value);
    }

    public void BackGroundSoundChange(float value)
    {
        GameAudioManager.currentBackGroundVolume = value;
    }

    public void SFXSoundChange(float value)
    {
        GameAudioManager.currentSFXVolume = value;
    }

    public void UISoundChange(float value)
    {
        GameAudioManager.currentUIVolume = value;
    }

    public void Mute(bool isOn)
    {
        if(isOn)
        {
            GameAudioManager.totalVolme = 0;
        }else
        {
            GameAudioManager.totalVolme = 1;
        }
        
    }
}

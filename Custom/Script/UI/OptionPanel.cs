using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionPanel : MonoBehaviour
{
    public void setFullScreen()
    {
        ResolutionManager.isFullScreen = !ResolutionManager.isFullScreen;
        ResolutionManager.SetFullScreen(ResolutionManager.isFullScreen);
    }

    public void ResolutionChange(Dropdown dropdown)
    {
        int value = dropdown.value;
        ResolutionManager.SetResolution(value);
    }
    public void LanguageChange(Dropdown dropdown)
    {
        int value = dropdown.value;
        
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

    public void Mute()
    {
        if(GameAudioManager.totalVolme > 0)
        {
            GameAudioManager.totalVolme = 0;
        }else
        {
            GameAudioManager.totalVolme = 1;
        }
        
    }
}

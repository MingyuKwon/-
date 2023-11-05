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

    public void BrightnessChange(float value)
    {
        BrightnessManager.instance.setBrightNess(value);
    }
}

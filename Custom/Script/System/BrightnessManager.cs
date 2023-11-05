using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BrightnessManager : MonoBehaviour
{
    public static BrightnessManager instance;
    public Image panel;

    private void Start() {
        instance = this;
    }

    public void setBrightNess(float i)
    {
        panel.color = new Color(0,0,0,(1-i) * 0.5f);
    }
}

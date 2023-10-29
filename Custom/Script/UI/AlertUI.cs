using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public interface AlertCallBack{
    public void CallBack();
}

public class AlertUI : MonoBehaviour
{
    public TextMeshProUGUI MainText;
    private AlertCallBack alertCallBack = null;

    public static AlertUI instance = null;
    // Start is called before the first frame update
    private void Awake() {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }else
        {
            Destroy(this.gameObject);
        }

        gameObject.SetActive(false);
    }

    public void YES()
    {
        alertCallBack.CallBack();
        CloseAlert();
    }

    public void NO()
    {   
        CloseAlert();
    }

    public void ShowAlert(string mainText, AlertCallBack callBackInstance)
    {
        MainText.text = mainText;
        alertCallBack = callBackInstance;
        gameObject.SetActive(true);
    }

    public void CloseAlert()
    {   
        gameObject.SetActive(false);
        MainText.text = "";
        alertCallBack = null;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;


public class InformationUI : MonoBehaviour
{
    public enum InformationSituation{
        startItemExplain = 0,
        defaultPlusItemExplain = 1,
        ItemPlusItemExplain = 2,
        item_obtain_Up_PercentageExplain = 3,
        TrapDamageExplain = 4,
        NoItemExplain = 5,
    }

    public string[] koreanExplations;
    public string[] englishExplations;

    public TextMeshProUGUI MainText;
    public static InformationUI instance = null;

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

    public void ShowInformation(InformationSituation situation)
    {
        if(LanguageManager.currentLanguage == "Korean")
        {
            MainText.text = koreanExplations[(int)situation];
        }else if(LanguageManager.currentLanguage == "English")
        {
            MainText.text = englishExplations[(int)situation];
        }
        
        gameObject.SetActive(true);
    }

    public void CloseInformationPanel()
    {   
        gameObject.SetActive(false);
        MainText.text = "";
    }
}
